#ifndef VIKINGCONSOLEDRIVERIMPL_H
#define VIKINGCONSOLEDRIVERIMPL_H
//----------------------------------------------------------------------------
//            Copyright (c) 2012 Covidien, Inc.
//
// This software is copyrighted by and is the sole property of Covidien. This
// is a proprietary work to which Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Covidien.
//----------------------------------------------------------------------------

#include "IVikingConsoleDriverImpl.h"

#include <Winsock2.h>
#include <Windows.h>
#include <memory>
#include <list>

typedef int socklen_t;


class VikingConsoleDriverImpl:public IVikingConsoleDriverImpl,
    public std::tr1::enable_shared_from_this<VikingConsoleDriverImpl>
{
	
	HANDLE VikingConsoleDriverDownloadCommandThreadHandle_;
    HANDLE VikingConsoleDriverThreadHandle_;
    HANDLE VikingConsoleDriverSemaphore_;
    HANDLE VikingConsoleDriverMutex_;

	DWORD VikingConsoleDriverDownloadCommandThreadlpThreadId_;
    DWORD VikingConsoleDriverThreadlpThreadId_;



	bool bTriggerEnabled;
    bool bRunning_;
    bool bFinished_;
	bool bDeviceExists_;
    unsigned int delay_;

    std::tr1::shared_ptr<VikingConsoleDriverImpl> reference_count_;
    WSADATA wsaData;
    int sock_serv;
    struct sockaddr_in sa_serv;
    struct sockaddr_in sa_cli;
    socklen_t addrlen_cli;
    int sock_opt;
    bool bStarted_;
	bool bListenConfig_;

    std::tr1::shared_ptr<IRemoteFileOperations> rmtOps;

    int (*OnThreadExit_)    (void *,char *,int len);
    void *objThreadExit_;

    int (*OnSocketError_)    (void *,char *,int len);
    void *objSocketError_;


    static int xmitMessage(void *in,char *str, int len);
    static int OnEverything(void *in,char *str, int len);
	static int  OnFileError_(void *in,char *str, int len);

    enum 
    {
        FN_OnThreadExit_,
        FN_OnSocketError_
    }; 






    std::list<std::shared_ptr<ITokenImpl> > tokenList_;
    std::tr1::shared_ptr<ITokenImpl> current_token;

    std::tr1::shared_ptr<VikingConsoleDriverImpl> getSharedPtr()
    {
        return shared_from_this();
    }

	int DownloadCommandThreadProc();
    int ThreadProc();

	static DWORD WINAPI  DownloadCommandThreadProc(LPVOID pVikingConsoleDriverImpl);
    static DWORD WINAPI  ThreadProc(LPVOID pVikingConsoleDriverImpl);
    static int OnMessage_(void *in,char *str, int len);



    public:

    VikingConsoleDriverImpl();
    ~VikingConsoleDriverImpl();


    void unregisterFn(unsigned int fnid);
    void registerFn(unsigned int fnid, int (* callback)(void *, char *,int), void *obj);


    static IVikingConsoleDriverImpl *init(wchar_t * path);
    void finish();
    void cleanUp();
    void reset();
    void setDelay(int i);


    int startDownloadServer();
    int stopDownloadServer();
	int getDownloadServerState();

	int enableTrigger();
	int disableTrigger();
	int getTriggerState();
	int oneShotTrigger();
	int isDeviceReady();
	void listenForConfig();
	int setDownloadServerWorkingPath(wchar_t *path);

    ITokenImpl *  getNextToken(unsigned long msec = 0);
};

#endif