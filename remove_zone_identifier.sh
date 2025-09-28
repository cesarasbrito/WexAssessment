#!/bin/sh
# remove_zone_identifier.sh
# Remove todos os arquivos que contenham ":Zone.Identifier" na pasta atual e subpastas

echo "Procurando arquivos com ':Zone.Identifier'..."

# Encontrar e remover
find . -type f -name '*:Zone.Identifier*' -exec rm -v {} \;

echo "Remoção concluída."
