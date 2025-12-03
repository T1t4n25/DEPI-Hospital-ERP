# Frontend-Backend Integration Guide

This document describes the integration structure between the Angular frontend and .NET backend API.

## Architecture Overview

The frontend follows a feature-based architecture matching the backend's vertical slice structure. Each feature has:
- **Models**: TypeScript interfaces matching backend DTOs
- **Services**: HTTP client services for API communication
- **Components**: UI components consuming the services

## API Integration Pattern

All features follow this consistent pattern:

### 1. Models Structure
Each feature has 4 model files:
- `{feature}-detail.model.ts` - Full entity details
- `{feature}-list.model.ts` - List view (minimal fields)
- `create-{feature}.model.ts` - Create operation
- `update-{feature}.model.ts` - Update operation (extends Create)

### 2. Service Pattern
All services implement:
- `getAll(params?: QueryParams): Observable<PaginatedResponse<T>>`
- `getById(id: number): Observable<T>`
- `create(data: CreateModel): Observable<T>`
- `update(id: number, data: UpdateModel): Observable<T>`
- `delete(id: number): Observable<void>`
- Caching with `shareReplay`
- Error handling with `catchError`
- Retry logic for transient failures

### 3. API Endpoints

All endpoints follow RESTful conventions:
- `GET /api/{feature}` - List (paginated)
- `GET /api/{feature}/{id}` - Get by ID
- `POST /api/{feature}` - Create
- `PUT /api/{feature}/{id}` - Update
- `DELETE /api/{feature}/{id}` - Delete

### 4. Authentication

All requests include JWT Bearer token via `authInterceptor`:
```
Authorization: Bearer {token}
```

## Features Integration Status

✅ **Completed:**
- Infrastructure (environments, interceptors, shared interfaces)
- Patients (models + service)

📋 **To Complete** (following Patients pattern):
- Services
- Departments
- Appointments
- Medical Records
- Employees
- Medications
- Inventory
- Invoices

## Quick Start

1. **Import service in component:**
```typescript
import { PatientsService } from '../services/patients.service';
private readonly patientsService = inject(PatientsService);
```

2. **Use in component:**
```typescript
this.patientsService.getAll({ pageNumber: 1, pageSize: 10 })
  .pipe(takeUntilDestroyed())
  .subscribe({
    next: (response) => this.items.set(response.data),
    error: (err) => this.error.set(err.message)
  });
```

## Environment Configuration

API URL is configured in:
- `src/environments/environment.ts` (development)
- `src/environments/environment.prod.ts` (production)

Default: `http://localhost:5037`

## Error Handling

All errors are caught by:
1. Service-level error handling (returns user-friendly messages)
2. Error interceptor (logs errors, handles 401/403)
3. Component-level handling (displays errors to users)

