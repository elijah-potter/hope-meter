#! /bin/bash

R=`pwd`

cd $R/site
yarn
yarn build

cp build ../wwwroot -r

cd $R

dotnet publish -c release
