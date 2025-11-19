# Build stage
FROM node:25-alpine AS build

# Update apk packages and install curl for healthchecks first for better layer caching
# This ensures we get the latest security patches
RUN apk update && apk upgrade && apk add --no-cache curl && rm -rf /var/cache/apk/*

WORKDIR /app

# Copy package files first for better layer caching
# This layer will be cached unless dependencies change
COPY package.json ./
# Copy package-lock.json if it exists, otherwise npm install will create it
COPY package-lock.json* ./
RUN if [ -f package-lock.json ]; then npm ci --only=production=false; else npm install --only=production=false; fi

# Copy all source code (this layer will invalidate cache when code changes)
COPY . .

# Build the Angular application
RUN npm run build -- --configuration production

# Runtime stage - use nginx to serve static files
FROM nginx:alpine AS final

# Update apk packages and install curl for healthchecks
# This ensures we get the latest security patches
RUN apk update && apk upgrade && apk add --no-cache curl && rm -rf /var/cache/apk/*

# Copy custom nginx configuration
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Copy built application from build stage
# Angular outputs to dist/<project-name>/browser by default
COPY --from=build /app/dist/frontend/browser /usr/share/nginx/html

# Expose port 80
EXPOSE 80

# Healthcheck
HEALTHCHECK --interval=30s --timeout=10s --retries=3 --start-period=10s \
CMD curl -f http://localhost:80 || exit 1

# Start nginx
CMD ["nginx", "-g", "daemon off;"]

