#!/bin/bash

version_manifest=$(curl -s -X 'GET' 'https://launchermeta.mojang.com/mc/game/version_manifest.json' -H 'accept: application/json' | jq -r '.versions[]')

releases=$(echo "$version_manifest" | jq -r 'select(.type == "release" ) | .id')

version_delimiter="."

for version in $releases; do
  if [ -d "./Resources/$version" ]; then
    continue
  fi

  IFS="$version_delimiter"
  read -ra version_parts <<< "$version"
  unset IFS
git remote add origin https://github.com/Shonz1/Void.Data.git
  if [ "${version_parts[0]}" == "1" ] && [ "${version_parts[1]}" -lt "14" ]; then
    echo "Version ${version} is below 1.14 and is not supported by the data generator, skipping..."
    continue
  fi

  version_info_url=$(echo "$version_manifest" | jq -r "select(.id == \"$version\") | .url")
  version_download_url=$(curl -s -X 'GET' "$version_info_url" -H 'accept: application/json' | jq -r '.downloads.server.url')
  if [ -z "$version_download_url" ]; then
    echo "No server download URL for ${version}, skipping..."
    continue
  fi

  echo "Downloading ${version}..."

  mkdir -p "./temp"
  cd "./temp" || exit 1

  curl -s -o "server.jar" "$version_download_url"

  echo "Downloaded ${version} server jar."

  echo "Extracting data for ${version}..."

  if [ "${version_parts[0]}" == "1" ] && [ "${version_parts[1]}" -lt "18" ]; then
    echo "Using legacy data extraction method for version ${version}..."
    java -cp server.jar net.minecraft.data.Main --reports >> /dev/null 2>&1
  else
    echo "Using modern data extraction method for version ${version}..."
    java -DbundlerMainClass=net.minecraft.data.Main -jar server.jar --reports --output ./generated >> /dev/null 2>&1
  fi

  cd ../ || exit 1

  mkdir -p "./Resources/$version"
  mv "./temp/generated"/* "./Resources/$version/"

  rm -rf "./temp"

  echo "Data extraction for ${version} completed."
done

