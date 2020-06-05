#!/bin/bash
dotnet exec \
	--depsfile ToDo.deps.json \
	--runtimeconfig ToDo.runtimeconfig.json \
	ef.dll database update \
	--assembly ToDo.dll \
	--startup-assembly ToDo.dll \
	--root-namespace ToDo
