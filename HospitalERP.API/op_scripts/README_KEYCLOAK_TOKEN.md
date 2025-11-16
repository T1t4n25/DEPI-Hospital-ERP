# Keycloak Token Generator

Python script to obtain JWT tokens from Keycloak using username/password authentication (Resource Owner Password Credentials grant).

## Prerequisites

1. Python 3.6 or higher
2. `requests` library: `pip install requests`
3. Keycloak server must be running
4. A client configured in Keycloak with Direct Access Grants enabled
5. A user account with username "test" and password "123qwe" (hardcoded in script)

## Installation

```bash
pip install requests
```

## Configuration

Add the following to your `.env` file (located in `HospitalERP.API/.env`):

```env
# Option 1: Using base URL and realm (recommended)
KEYCLOAK_BASE_URL=http://localhost:8080
KEYCLOAK_REALM=hospital-erp
KEYCLOAK_CLIENT_ID=hospital-erp-service-client
# Client secret is optional (only needed for confidential clients)
KEYCLOAK_CLIENT_SECRET=your-client-secret-here

# Option 2: Using authority (also supported)
KEYCLOAK_AUTHORITY=http://localhost:8080/auth/realms/hospital-erp
KEYCLOAK_CLIENT_ID=hospital-erp-service-client
KEYCLOAK_CLIENT_SECRET=your-client-secret-here
```

**Note:** The script uses hardcoded username "test" and password "123qwe". Ensure this user exists in Keycloak.

## Usage

### Basic Usage

```bash
# Navigate to project root (where .env is located)
cd HospitalERP.API

# Default output file (token.txt)
python op_scripts/get-keycloak-token.py

# Custom output file
python op_scripts/get-keycloak-token.py my-token.txt
```

### Make executable (optional)

```bash
# Make executable (Linux/macOS)
chmod +x HospitalERP.API/op_scripts/get-keycloak-token.py
cd HospitalERP.API
./op_scripts/get-keycloak-token.py
```

## Features

- ✅ Uses username/password authentication (hardcoded: test/123qwe)
- ✅ Supports optional client secret for confidential clients
- ✅ Reads configuration from `.env` file
- ✅ Supports both `KEYCLOAK_BASE_URL` + `KEYCLOAK_REALM` and `KEYCLOAK_AUTHORITY`
- ✅ Extracts and displays roles from token (realm_access and resource_access)
- ✅ Decodes and displays JWT token information
- ✅ Shows token expiration time
- ✅ Saves token to file
- ✅ Error handling with helpful messages
- ✅ Token preview and usage examples

## Output

The script will:
1. Load environment variables from `.env` file
2. Authenticate with Keycloak using username/password (test/123qwe)
3. Extract and display roles from realm_access and resource_access claims
4. Decode and display token information (claims, expiration, etc.)
5. Save the JWT access token to a text file
6. Show usage examples

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

- Ensure `.env` file exists in `HospitalERP.API/.env`
- Copy from `HospitalERP.API/.env.example` if needed

### Error: Missing required environment variables

Check that all required variables are set in `HospitalERP.API/.env`:
- `KEYCLOAK_CLIENT_ID` (required)
- `KEYCLOAK_CLIENT_SECRET` (optional, only for confidential clients)
- `KEYCLOAK_BASE_URL` (or `KEYCLOAK_AUTHORITY`)

### Error: HTTP 401 Unauthorized

- Verify the user "test" with password "123qwe" exists in Keycloak
- Check that client has "Direct Access Grants" enabled (for password grant)
- Verify `KEYCLOAK_CLIENT_SECRET` matches the client secret in Keycloak (if using confidential client)
- Ensure the client allows password grant type

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
    ['python', 'HospitalERP.API/op_scripts/get-keycloak-token.py', 'token.txt'],
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
cd HospitalERP.API
export KEYCLOAK_CLIENT_ID=my-client-id
export KEYCLOAK_CLIENT_SECRET=my-secret
python op_scripts/get-keycloak-token.py
```

