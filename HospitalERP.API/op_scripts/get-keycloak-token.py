#!/usr/bin/env python3
"""
Script to get JWT token from Keycloak using username/password authentication.
Usage: python scripts/get-keycloak-token.py [output-file]
"""

import os
import sys
import json
import base64
import requests
from pathlib import Path
from typing import Optional
from pprint import pprint


def load_env_file(env_file: str = ".env") -> dict:
    """Load environment variables from .env file."""
    env_vars = {}
    env_path = Path(env_file)
    
    if not env_path.exists():
        print(f"❌ Error: .env file not found at {env_file}")
        sys.exit(1)
    
    with open(env_path, 'r') as f:
        for line in f:
            line = line.strip()
            # Skip empty lines and comments
            if not line or line.startswith('#'):
                continue
            # Parse KEY=VALUE format
            if '=' in line:
                key, value = line.split('=', 1)
                env_vars[key.strip()] = value.strip().strip('"\'')
    
    return env_vars


def get_keycloak_token(
    base_url: str,
    realm: str,
    client_id: str,
    username: str,
    password: str,
    client_secret: Optional[str] = None,
    grant_type: str = "password"
) -> str:
    """
    Get JWT token from Keycloak using username/password (Resource Owner Password Credentials grant).
    
    Args:
        base_url: Keycloak base URL
        realm: Realm name
        client_id: Client ID
        username: Username for authentication
        password: Password for authentication
        client_secret: Client Secret (optional, required for confidential clients)
        grant_type: Grant type (default: password)
    
    Returns:
        Access token as string
    """
    url = f"{base_url}/realms/{realm}/protocol/openid-connect/token"
    
    data = {
        "grant_type": grant_type,
        "client_id": client_id,
        "username": username,
        "password": password,
        "scope": "openid profile email",
    }
    
    # Add client_secret if provided (required for confidential clients)
    if client_secret:
        data["client_secret"] = client_secret
    
    headers = {"Content-Type": "application/x-www-form-urlencoded"}
    
    try:
        response = requests.post(url, data=data, headers=headers, timeout=10)
        response.raise_for_status()
        
        token_data = response.json()
        return token_data["access_token"]
    
    except requests.exceptions.HTTPError as e:
        print(f"❌ HTTP Error: {e.response.status_code}")
        print(f"Response: {e.response.text}")
        sys.exit(1)
    except requests.exceptions.RequestException as e:
        print(f"❌ Request Error: {e}")
        sys.exit(1)
    except KeyError:
        print("❌ Error: 'access_token' not found in response")
        print(f"Response: {response.json()}")
        sys.exit(1)


def decode_jwt_payload(token: str) -> Optional[dict]:
    """Decode JWT token payload (without verification)."""
    try:
        # JWT has three parts: header.payload.signature
        parts = token.split('.')
        if len(parts) != 3:
            return None
        
        payload = parts[1]
        
        # Add padding if needed
        padding = 4 - len(payload) % 4
        if padding != 4:
            payload += '=' * padding
        
        # Decode base64
        decoded_bytes = base64.urlsafe_b64decode(payload)
        decoded_json = json.loads(decoded_bytes.decode('utf-8'))
        
        return decoded_json
    except Exception as e:
        print(f"⚠️  Could not decode token payload: {e}")
        return None


def format_token_info(payload: dict) -> None:
    """Format and display token information."""
    print("\n📋 Token Information:")
    print("=" * 50)
    
    # Common JWT claims
    if 'sub' in payload:
        print(f"Subject (sub): {payload['sub']}")
    if 'iss' in payload:
        print(f"Issuer (iss): {payload['iss']}")
    if 'aud' in payload:
        audience = payload['aud']
        if isinstance(audience, list):
            audience = ', '.join(audience)
        print(f"Audience (aud): {audience}")
    if 'exp' in payload:
        import datetime
        exp_time = datetime.datetime.fromtimestamp(payload['exp'])
        print(f"Expires (exp): {exp_time.strftime('%Y-%m-%d %H:%M:%S')}")
    if 'iat' in payload:
        import datetime
        iat_time = datetime.datetime.fromtimestamp(payload['iat'])
        print(f"Issued At (iat): {iat_time.strftime('%Y-%m-%d %H:%M:%S')}")
    if 'realm_access' in payload:
        print(f"Realm Access: {payload['realm_access']}")
    if 'resource_access' in payload:
        print(f"Resource Access: {payload['resource_access']}")
    
    print("=" * 50)


def main():
    """Main function."""
    # Get output file from command line argument or use default
    output_file = sys.argv[1] if len(sys.argv) > 1 else "token.txt"
    
    # Load environment variables
    print("✓ Loading environment variables from .env...")
    env_vars = load_env_file()
    
    # Get required configuration
    base_url = env_vars.get("KEYCLOAK_BASE_URL") or env_vars.get("KEYCLOAK_AUTHORITY", "").replace("/realms/hospital-erp", "")
    realm = env_vars.get("KEYCLOAK_REALM", "hospital-erp")
    client_id = env_vars.get("KEYCLOAK_CLIENT_ID")
    client_secret = env_vars.get("KEYCLOAK_CLIENT_SECRET")  # Optional for confidential clients
    
    # If KEYCLOAK_AUTHORITY is set, extract base URL and realm
    if not base_url and "KEYCLOAK_AUTHORITY" in env_vars:
        authority = env_vars["KEYCLOAK_AUTHORITY"]
        # Extract base URL (remove /auth/realms/{realm})
        if "/auth/realms/" in authority:
            base_url = authority.split("/auth/realms/")[0]
            realm = authority.split("/auth/realms/")[1].rstrip("/")
    
    # Check if required variables are set
    if not base_url:
        print("❌ Error: KEYCLOAK_BASE_URL or KEYCLOAK_AUTHORITY not set in .env")
        sys.exit(1)
    
    if not client_id:
        print("❌ Error: KEYCLOAK_CLIENT_ID not set in .env")
        print("Please set the following in your .env file:")
        print("  - KEYCLOAK_CLIENT_ID")
        print("  - KEYCLOAK_BASE_URL (or KEYCLOAK_AUTHORITY)")
        print("  - KEYCLOAK_CLIENT_SECRET (optional, required for confidential clients)")
        sys.exit(1)
    
    # Username and password (hardcoded as requested)
    username = "test"
    password = "123qwe"
    
    # Display configuration
    print(f"✓ Configuration loaded:")
    print(f"  Base URL: {base_url}")
    print(f"  Realm: {realm}")
    print(f"  Client ID: {client_id}")
    if client_secret:
        print(f"  Client Secret: {'*' * len(client_secret)} (configured)")
    else:
        print(f"  Client Secret: (not set - using public client)")
    print(f"  Username: {username}")
    print(f"\n🔐 Requesting JWT token from Keycloak...")
    
    # Get token
    try:
        token = get_keycloak_token(base_url, realm, client_id, username, password, client_secret)
        print("✓ Token received successfully!")
        
        # Decode and display token info
        payload = decode_jwt_payload(token)
        if payload:
            format_token_info(payload)
        
        # Save token to file
        output_path = Path(output_file)
        with open(output_path, 'w') as f:
            f.write(token)
        
        print(f"\n✓ Token saved to: {output_path.absolute()}")
        print(f"  Token preview: {token[:50]}...")
        print(f"\n💡 Usage example:")
        print(f"  import requests")
        print(f"  with open('{output_file}') as f:")
        print(f"      token = f.read().strip()")
        print(f"  headers = {{'Authorization': f'Bearer {{token}}'}}")
        print(f"  response = requests.get('http://localhost:5000/api/patients', headers=headers)")
        
    except Exception as e:
        print(f"❌ Error: {e}")
        sys.exit(1)


if __name__ == "__main__":
    main()

