# Keycloak Token Generator

Python script to obtain JWT tokens from Keycloak using service client credentials (client credentials grant).

## Prerequisites

1. Python 3.6 or higher
2. `requests` library: `pip install requests`
3. Keycloak server must be running
4. A service client configured in Keycloak

## Installation

```bash
pip install requests
```

## Configuration

Add the following to your `.env` file:

```env
# Option 1: Using base URL and realm (recommended)
KEYCLOAK_BASE_URL=http://localhost:8080
KEYCLOAK_REALM=hospital-erp
KEYCLOAK_CLIENT_ID=hospital-erp-service-client
KEYCLOAK_CLIENT_SECRET=your-service-client-secret-here

# Option 2: Using authority (also supported)
KEYCLOAK_AUTHORITY=http://localhost:8080/auth/realms/hospital-erp
KEYCLOAK_CLIENT_ID=hospital-erp-service-client
KEYCLOAK_CLIENT_SECRET=your-service-client-secret-here
```

## Usage

### Basic Usage

```bash
# Default output file (token.txt)
python scripts/get-keycloak-token.py

# Custom output file
python scripts/get-keycloak-token.py my-token.txt
```

### Make executable (optional)

```bash
# Make executable (Linux/macOS)
chmod +x scripts/get-keycloak-token.py
./scripts/get-keycloak-token.py
```

## Features

- ✅ Reads configuration from `.env` file
- ✅ Supports both `KEYCLOAK_BASE_URL` + `KEYCLOAK_REALM` and `KEYCLOAK_AUTHORITY`
- ✅ Decodes and displays JWT token information
- ✅ Shows token expiration time
- ✅ Saves token to file
- ✅ Error handling with helpful messages
- ✅ Token preview and usage examples

## Output

The script will:
1. Load environment variables from `.env`
2. Authenticate with Keycloak using client credentials
3. Decode and display token information (claims, expiration, etc.)
4. Save the JWT access token to a text file
5. Show usage examples

### Example Output

```
✓ Loading environment variables from .env...
✓ Configuration loaded:
  Base URL: http://localhost:8080
  Realm: hospital-erp
  Client ID: hospital-erp-service-client

🔐 Requesting JWT token from Keycloak...
✓ Token received successfully!

📋 Token Information:
==================================================
Subject (sub): service-account-hospital-erp-service-client
Issuer (iss): http://localhost:8080/realms/hospital-erp
Audience (aud): hospital-erp-api
Expires (exp): 2024-11-14 23:30:00
Issued At (iat): 2024-11-14 23:00:00
Realm Access: {'roles': ['offline_access', 'uma_authorization']}
==================================================

✓ Token saved to: /path/to/token.txt
  Token preview: eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJ...

💡 Usage example:
  import requests
  with open('token.txt') as f:
      token = f.read().strip()
  headers = {'Authorization': f'Bearer {token}'}
  response = requests.get('http://localhost:5000/api/patients', headers=headers)
```

## Using the Token

### Python

```python
import requests

# Read token from file
with open('token.txt') as f:
    token = f.read().strip()

# Use in API requests
headers = {'Authorization': f'Bearer {token}'}
response = requests.get('http://localhost:5000/api/patients', headers=headers)
print(response.json())
```

### cURL

```bash
TOKEN=$(cat token.txt)
curl -H "Authorization: Bearer $TOKEN" http://localhost:5000/api/patients
```

### PowerShell

```powershell
$token = Get-Content token.txt
Invoke-RestMethod -Uri "http://localhost:5000/api/patients" `
    -Headers @{Authorization = "Bearer $token"}
```

## Troubleshooting

### Error: requests module not found

```bash
pip install requests
```

### Error: .env file not found

- Ensure `.env` file exists in the project root
- Copy from `.env.example` if needed

### Error: Missing required environment variables

Check that all required variables are set in `.env`:
- `KEYCLOAK_CLIENT_ID`
- `KEYCLOAK_CLIENT_SECRET`
- `KEYCLOAK_BASE_URL` (or `KEYCLOAK_AUTHORITY`)

### Error: HTTP 401 Unauthorized

- Verify `KEYCLOAK_CLIENT_SECRET` matches the client secret in Keycloak
- Check that client has "Client credentials" grant enabled
- Verify client has "Service accounts roles" enabled

### Error: Connection refused

- Verify Keycloak is running
- Check `KEYCLOAK_BASE_URL` or `KEYCLOAK_AUTHORITY` is correct
- Ensure network connectivity

## Advanced Usage

### Using in Python Scripts

```python
import subprocess
import json

# Get token programmatically
result = subprocess.run(
    ['python', 'scripts/get-keycloak-token.py', 'token.txt'],
    capture_output=True,
    text=True
)

if result.returncode == 0:
    with open('token.txt') as f:
        token = f.read().strip()
    # Use token...
else:
    print(f"Error: {result.stderr}")
```

### Environment Variable Override

You can override `.env` values with environment variables:

```bash
export KEYCLOAK_CLIENT_ID=my-client-id
export KEYCLOAK_CLIENT_SECRET=my-secret
python scripts/get-keycloak-token.py
```

