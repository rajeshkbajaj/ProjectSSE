@set JVM_DLL="C:\Java\jre6\bin\client\jvm.dll"
@set CLASSPATH=%cd%\lib\jersey-apache-client-1.8.jar;%cd%\lib\jsr311-api-1.1.1.jar
@set CLASSPATH=%CLASSPATH%;%cd%\lib\commons-httpclient-3.0.1.jar;%cd%\lib\commons-logging-1.1.1.jar
@set CLASSPATH=%CLASSPATH%;%cd%\lib\commons-codec-1.4.jar;%cd%\lib\jersey-multipart-1.12.jar
@set CLASSPATH=%CLASSPATH%;%cd%\lib\netty-3.5.2.Final.jar;%cd%\lib\log4j-1.2.17.jar
@set CLASSPATH=%CLASSPATH%;%cd%\lib\jasypt-1.9.0.jar;%cd%\lib\dcpscj.jar;%cd%\lib\jersey-bundle-1.12.jar
@set CLASSPATH=%CLASSPATH%;%cd%\LaptopAgent.jar
java com.covidien.AgentRunnable start


