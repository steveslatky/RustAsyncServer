EXE = Client.exe

all: run
.PHONY: clean

build:
	mcs Class1.cs -out:Client.exe /reference:Newtonsoft.Json.dll /reference:System.Runtime.dll

run: build
	mono Client.exe

clean:
	-rm $(EXE)
