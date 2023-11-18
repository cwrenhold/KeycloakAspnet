#!/bin/bash

# Wait 30 seconds for Keycloak to start
sleep 30

# Keycloak server URL
KEYCLOAK_URL="http://keycloak:8080/auth"

# Admin credentials
ADMIN_USERNAME=${KEYCLOAK_USER}
ADMIN_PASSWORD=${KEYCLOAK_PASSWORD}
ASPNET_ROOT_URL="https://localhost:${ASPNET_HTTPS_PORT}"

# Function to obtain an admin token
get_admin_token() {
  # Obtain token
  local TOKEN_RESPONSE=$(curl -s -k \
    -d "client_id=admin-cli" \
    -d "username=$ADMIN_USERNAME" \
    -d "password=$ADMIN_PASSWORD" \
    -d "grant_type=password" \
    "$KEYCLOAK_URL/realms/master/protocol/openid-connect/token")

  # Parse token
  ADMIN_TOKEN=$(echo $TOKEN_RESPONSE | jq -r '.access_token')
}

# Function to create a new realm
create_realm() {
  local REALM_NAME=$1
  curl -s -k \
    -X POST \
    -H "Authorization: Bearer $ADMIN_TOKEN" \
    -H "Content-Type: application/json" \
    -d '{"id":"'"$REALM_NAME"'","realm":"'"$REALM_NAME"'","enabled":true,"registrationAllowed":true,"loginWithEmailAllowed":false}' \
    "$KEYCLOAK_URL/admin/realms"
}

# Function to create a new client
create_client() {
  local REALM_NAME=$1
  local CLIENT_NAME=$2
  local CLIENT_SECRET=$3
  curl -s -k \
    -X POST \
    -H "Authorization: Bearer $ADMIN_TOKEN" \
    -H "Content-Type: application/json" \
    -d '{"clientId":"'"$CLIENT_NAME"'","secret":"'"$CLIENT_SECRET"'","directAccessGrantsEnabled":true,"enabled":true,"publicClient":false,"redirectUris":["*"],"rootUrl":"'"$ASPNET_ROOT_URL"'","baseUrl": "/"}' \
    "$KEYCLOAK_URL/admin/realms/$REALM_NAME/clients"
}

# Main script execution
# Get admin token
get_admin_token

# Create a new realm
create_realm "${KEYCLOAK_REALM}"

# Create a new client
create_client "${KEYCLOAK_REALM}" "${KEYCLOAK_CLIENT_ID}" "${KEYCLOAK_CLIENT_SECRET}"

# Add additional configuration as needed
echo "Keycloak configuration complete."
