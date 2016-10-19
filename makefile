SHELL:=/bin/bash
ifeq ($(OS), Windows_NT)
	UNITY_PATH = "C:\Program Files\Unity\Editor\Unity.exe"
else
	UNITY_PATH = /Applications/Unity/Unity.app/Contents/MacOS/Unity
endif

WORKING_DIR := $(dir $(abspath $(lastword $(MAKEFILE_LIST))))

default: test build

test:
	@if [ ! -d "Assets/UnityTestTools" ]; then \
		echo "Cannot run unit tests. Please import Unity Test Tools from the asset store and try again."; \
	else \
		echo -n "Running unit tests..."; \
		\
		while : ; sleep 2s; do echo -n "."; done & DOTS=$$!; \
		$(UNITY_PATH) -batchmode \
		-projectPath $(WORKING_DIR) \
		-executeMethod UnityTest.Batch.RunIntegrationTests \
		-testscenes=UnitTests \
		-resultsFileDirectory=./; \
		kill -9 $$DOTS && wait $$DOTS 2> /dev/null; \
		\
		echo ""; \
		\
		grep '<test-case' UnitTests.xml | sed -e 's,.*<test-case\([^<]*\)>.*,\1,g'; \
		rm UnitTests.xml; \
		echo ""; \
	fi;

build:
	@echo "Exporting pullstring.unitypackage"; \
	$(UNITY_PATH) -batchmode -quit \
	-projectPath $(WORKING_DIR) \
	-exportPackage Assets/Scripts/PullString pullstring.unitypackage; \
	echo "done";
