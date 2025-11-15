#!/bin/bash
# Load .env file and export variables
# Usage: source scripts/load-env.sh

if [ -f .env ]; then
    export $(cat .env | grep -v '^#' | xargs)
    echo "Environment variables loaded from .env"
else
    echo "Warning: .env file not found"
fi

