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



#include "VikingConsoleDriverImpl.h"
#include "RemoteFileOperations.h"
#include "NetworkMessageHeader.h"
#include "TokenImpl.h"
#include "CommandInfo.h"
#include <sstream>
#include <fstream>



int VikingConsoleDriverImpl::OnEverything(void *in,char *str, int len)
{
    return 0;
}

void VikingConsoleDriverImpl::unregisterFn(unsigned int fnid)
{
    switch(fnid)
    {

        case FN_OnThreadExit_:
            OnThreadExit_ = OnEverything;
            break;
        case FN_OnSocketError_:
            OnSocketError_ = OnEverything;
            break;

    }
}


void VikingConsoleDriverImpl::registerFn(unsigned int fnid, int (* callback)(void *, char *,int), void *obj)
{
    switch(fnid)
    {
        case FN_OnThreadExit_:
            OnThreadExit_ = callback;
            objThreadExit_= obj;
            break;

        case FN_OnSocketError_:
            OnSocketError_ = callback;
            objSocketError_ = obj;
            break;
    }

}




VikingConsoleDriverImpl::VikingConsoleDriverImpl():
    VikingConsoleDriverThreadHandle_(0),
    VikingConsoleDriverSemaphore_(0),
    VikingConsoleDriverMutex_(0),
    VikingConsoleDriverThreadlpThreadId_(0),
    OnThreadExit_(OnEverything),
    OnSocketError_(OnEverything),
	bTriggerEnabled(false),
    bRunning_(false),
    bFinished_(false),
	bDeviceExists_(false),
    delay_(0),
    bStarted_(false),bListenConfig_(false),rmtOps(new RemoteFileOperations())

{
    this->rmtOps->registerFn(RemoteFileOperations::FN_OnReply_,xmitMessage,this);
    this->rmtOps->registerFn(RemoteFileOperations::FN_OnStderrWrite_, OnMessage_,this);
    this->rmtOps->registerFn(RemoteFileOperations::FN_OnStdoutWrite_, OnMessage_,this);
	this->rmtOps->registerFn(RemoteFileOperations::FN_OnFileError_, OnFileError_,this);

}

VikingConsoleDriverImpl::~VikingConsoleDriverImpl()
{
}

void VikingConsoleDriverImpl::listenForConfig()
{
	bListenConfig_ = true;
}

int VikingConsoleDriverImpl::OnMessage_(void *in,char *str, int len)
{

    //If there is nothing to precess, return.
    if(len == 0 || in == 0)
    {
        return 0;
    }

    VikingConsoleDriverImpl *pVikingConsoleDriverImpl=(VikingConsoleDriverImpl *)in;

	if(str[len-1] == '\n')
	{
		len--;
	}
  //  std::string line(str,str+len);


   

	std::string message(str,str+len);

	std::stringstream ss(message);
	std::string s;

	while (std::getline(ss, s)) 
    {
        
        std::tr1::shared_ptr<TokenImpl> newToken(new TokenImpl());

		std::stringstream line(s); 

		std::string sub;

		bool bEr = false;

        while(std::getline(line, sub, ',')){

            if(sub.compare("")!=0)
            {
                newToken->insertValue(sub);
            }
        } 

        //If the token has length, insert
        if(newToken->getLength())
        {

            while(WaitForSingleObject(pVikingConsoleDriverImpl->VikingConsoleDriverMutex_, 500 ) == WAIT_TIMEOUT)
            {
                if(pVikingConsoleDriverImpl->bRunning_ == false)
                {
                    break;
                }
                Sleep(5);
            }


            if(pVikingConsoleDriverImpl->bRunning_)
            {
                pVikingConsoleDriverImpl->tokenList_.push_back(newToken);

                if(pVikingConsoleDriverImpl->tokenList_.size() > 100)
                {
                    pVikingConsoleDriverImpl->tokenList_.pop_front();
					bEr=true;
                }


                ReleaseSemaphore (pVikingConsoleDriverImpl->VikingConsoleDriverSemaphore_,1,NULL);
            }

            ReleaseMutex(pVikingConsoleDriverImpl->VikingConsoleDriverMutex_);
        }

		//If we are deleting elements, we should yeild for a few cycles.
		if(bEr)
		{
			Sleep(25);
			bEr = false;
		}
    }



    return 0;
}


#include <string.h>
int  VikingConsoleDriverImpl::OnFileError_(void *in,char *str, int len)
{
	char buffer[400];

	VikingConsoleDriverImpl *pVikingConsoleDriverImpl=(VikingConsoleDriverImpl *)in;


	sprintf_s(buffer,sizeof(buffer)-1,"REMOTEFILEIO,ERROR,%s",str);

	return VikingConsoleDriverImpl::OnMessage_((void *)pVikingConsoleDriverImpl,str,strnlen(buffer,sizeof(buffer)-1));
}


void VikingConsoleDriverImpl::finish()
{
    if((bFinished_ == false) && (bRunning_ == true))
    {
        bRunning_ = false;
		bListenConfig_ = false;
        DWORD dwExitCode = 0;
        BOOL bRet = false;
        int cnt =10;

        if(sock_serv)
        {
            shutdown(sock_serv,SD_BOTH);
            closesocket(sock_serv);
            sock_serv=0;
        }


        while( (bFinished_==false && cnt) 
                && (bRet=(GetExitCodeThread(VikingConsoleDriverThreadHandle_, &dwExitCode) != STILL_ACTIVE))
             )
        {
            Sleep(100);
            cnt--;
        }

        //Focebly kill the thread
        if(bRet == false)
        {
            Sleep(100);
            TerminateThread(VikingConsoleDriverThreadHandle_,dwExitCode);
        }
        cleanUp();

        this->reference_count_.reset();
    }
}


int VikingConsoleDriverImpl::ThreadProc()
{

    bFinished_ = false;
    bRunning_  = true;


    //TODO[ME]: make registerFn, unregister and using fn thread safe

    while(bRunning_)
    {
		if(delay_!=0)
		{
			Sleep(delay_);
		}

        char recv_buffer[3000];
        addrlen_cli = (socklen_t) sizeof(sa_cli);
        int recv_len = recvfrom(sock_serv, recv_buffer, sizeof(recv_buffer), 0,
                (struct sockaddr *)&sa_cli, &addrlen_cli);
		if(recv_len > 0)
		{
			//received something from the remote device
			bDeviceExists_ = true;
		}

        if(recv_len>0)
        {
			if( bStarted_)
				int retVal = this->rmtOps->process_msg(recv_buffer,recv_len);
			else if(bListenConfig_)
				int retVal = this->rmtOps->process_config(recv_buffer,recv_len);
        }



    }


    bFinished_ = true;
    return 0;
}


int VikingConsoleDriverImpl::DownloadCommandThreadProc()
{
	char xmessage [sizeof(CommandInfo)+sizeof(NetworkMessageHeader)];

	const Download::uuid_t REMOTE_INSTALLER_SYNC 
		={0x1b,0x1e,0xad,0x4a,0x46,0xeb,0x42,0xca,0xaa,0xb9,0xdc,0xdf,0x9b,0xea,0xde,0xcb};


	((NetworkMessageHeader*)xmessage) ->iLength
		 = htonl(sizeof(NetworkMessageHeader)+sizeof(CommandInfo));

	((NetworkMessageHeader*)xmessage) ->iOffset=0;


	CommandInfo *pTemp = (CommandInfo *)(((char *)xmessage)+sizeof(NetworkMessageHeader));
	
	memcpy((char *)pTemp->TypeUUID, REMOTE_INSTALLER_SYNC, sizeof(uuid_t));


	struct sockaddr_in si_other;
	int s,  slen=sizeof(si_other);

	s=socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
            memset((char *) &si_other, 0, sizeof(si_other));
    si_other.sin_family = AF_INET;
    si_other.sin_port = htons(9930);
    si_other.sin_addr.s_addr = inet_addr("192.168.0.12");


	while(true)
	{
		Sleep(1000);

		if((bTriggerEnabled ==true) && (bStarted_ == true))
		{
		
			sendto(s, xmessage, sizeof(xmessage), 0, (sockaddr *)&si_other, slen);
		}

	}
}

DWORD WINAPI  VikingConsoleDriverImpl::DownloadCommandThreadProc(LPVOID pVikingConsoleDriverImpl)
{
    return ((VikingConsoleDriverImpl *)pVikingConsoleDriverImpl)->DownloadCommandThreadProc();
}


DWORD WINAPI  VikingConsoleDriverImpl::ThreadProc(LPVOID pVikingConsoleDriverImpl)
{

    return ((VikingConsoleDriverImpl *)pVikingConsoleDriverImpl)->ThreadProc();
}


int VikingConsoleDriverImpl::xmitMessage(void *in,char *str, int len)
{
    if((str==0) || (len == 0) )
    {
        return 0;
    }

    VikingConsoleDriverImpl *rImpl = (VikingConsoleDriverImpl *)in;

    if(sendto(rImpl->sock_serv, str, len,
                0,(struct sockaddr *)&rImpl->sa_cli, rImpl->addrlen_cli)<1)
    {
        rImpl->OnSocketError_(rImpl->objSocketError_,"Sendto Error",14);
    }

    //TODO[ME]: Review all return values.
    return 0;
}


IVikingConsoleDriverImpl *VikingConsoleDriverImpl::init(wchar_t * path)
{
    bool dwRet = CreateDirectoryW(path, NULL);
	
	dwRet = SetCurrentDirectoryW(path);

	if(dwRet == false)
		return 0;

    int iResult;
    WSADATA wsaData;
    // Initialize Winsock
    iResult = WSAStartup(MAKEWORD(2,2), &wsaData);
    if (iResult != 0) {
        printf("WSAStartup failed: %d\n", iResult);
        return 0;
    }

    std::tr1::shared_ptr<VikingConsoleDriverImpl> pVikingConsoleDriverImpl(new VikingConsoleDriverImpl());

	VikingConsoleDriverImpl *pTempVikingConsoleDriverImpl=pVikingConsoleDriverImpl.get();


    pTempVikingConsoleDriverImpl->wsaData = wsaData;


    /* Create the socket. */
    pTempVikingConsoleDriverImpl->sock_serv = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);

    printf("%x\n",pTempVikingConsoleDriverImpl->sock_serv);
    if (pVikingConsoleDriverImpl->sock_serv < 0)
    {
        perror("socket");
        exit(EXIT_FAILURE);
    }
    /* Alloc for socket to be reused */
    pTempVikingConsoleDriverImpl->sock_opt = 1;
    setsockopt(pTempVikingConsoleDriverImpl->sock_serv, SOL_SOCKET, SO_REUSEADDR, (const char *)&pTempVikingConsoleDriverImpl->sock_opt,
            sizeof(pTempVikingConsoleDriverImpl->sock_opt));

    /* Give the socket a name. */
    pTempVikingConsoleDriverImpl->sa_serv.sin_family = AF_INET;
    pTempVikingConsoleDriverImpl->sa_serv.sin_port = htons(8080);
    pTempVikingConsoleDriverImpl->sa_serv.sin_addr.s_addr = htonl(INADDR_ANY);



	{
		/* Bind socket to port */
		if (::bind(pTempVikingConsoleDriverImpl->sock_serv, (struct sockaddr *)&pTempVikingConsoleDriverImpl->sa_serv, sizeof(struct sockaddr_in)) < 0)
		{
			perror("bind");
			exit(EXIT_FAILURE);
		}
	
	}

    fprintf(stderr,"Server is bound to port %d.\n", ntohs(pTempVikingConsoleDriverImpl->sa_serv.sin_port));


    pTempVikingConsoleDriverImpl->VikingConsoleDriverSemaphore_ =
        CreateSemaphore(NULL,65535,65535,L"VikingConsoleDriverSemaphore");

    if(pVikingConsoleDriverImpl->VikingConsoleDriverSemaphore_==NULL)
    {
        goto cleanup;
    }

    pTempVikingConsoleDriverImpl->VikingConsoleDriverMutex_ =
        CreateMutex(NULL,FALSE,L"VikingConsoleDriverMutex");

    if(pTempVikingConsoleDriverImpl->VikingConsoleDriverMutex_==NULL)
    {
        goto cleanup;
    }


	VikingConsoleDriverImpl *ptr = pVikingConsoleDriverImpl.get();


	pTempVikingConsoleDriverImpl->VikingConsoleDriverDownloadCommandThreadHandle_=
			CreateThread( NULL, 0, VikingConsoleDriverImpl::DownloadCommandThreadProc, ptr, 0,
                &ptr->VikingConsoleDriverDownloadCommandThreadlpThreadId_);


	pTempVikingConsoleDriverImpl->VikingConsoleDriverThreadHandle_=
        CreateThread( NULL, 0, VikingConsoleDriverImpl::ThreadProc, ptr, 0,
                &ptr->VikingConsoleDriverThreadlpThreadId_);

	

    if(pTempVikingConsoleDriverImpl->VikingConsoleDriverThreadHandle_==NULL)
    {
        goto cleanup;
    }



    pTempVikingConsoleDriverImpl->reference_count_ = pVikingConsoleDriverImpl->getSharedPtr();


    return pTempVikingConsoleDriverImpl;



cleanup:

    pTempVikingConsoleDriverImpl->cleanUp();


    return 0;
}

void VikingConsoleDriverImpl::cleanUp()
{
    if(VikingConsoleDriverThreadlpThreadId_)
    {
        DWORD dwExitCode = 0;
		TerminateThread(this->VikingConsoleDriverDownloadCommandThreadHandle_,dwExitCode);
		TerminateThread(this->VikingConsoleDriverThreadHandle_,dwExitCode);
        VikingConsoleDriverThreadlpThreadId_=0;
        bFinished_ = true;
        bRunning_  = false;
		bListenConfig_ = false;
    }

    if(VikingConsoleDriverMutex_)
    {
        CloseHandle(VikingConsoleDriverMutex_);
        VikingConsoleDriverMutex_ = 0;
    }

    if(VikingConsoleDriverSemaphore_)
    {
        CloseHandle(VikingConsoleDriverSemaphore_);
        VikingConsoleDriverSemaphore_ = 0;
    }

    if(sock_serv)
    {
        closesocket(sock_serv);
    }

    WSACleanup();

}


void VikingConsoleDriverImpl::setDelay(int i)
{
    delay_=i;
}

int VikingConsoleDriverImpl::enableTrigger()
{
	bTriggerEnabled=true;
	return 0;
}

int VikingConsoleDriverImpl::disableTrigger()
{
	bTriggerEnabled=false;
	return 0;
}

int VikingConsoleDriverImpl::getTriggerState()
{
	return bTriggerEnabled?1:0;
}

int VikingConsoleDriverImpl::getDownloadServerState()
{
	return bStarted_?1:0;
}


int VikingConsoleDriverImpl::oneShotTrigger()
{
	if( (bTriggerEnabled==false) && (bStarted_ == true) )
	{
		char xmessage [sizeof(CommandInfo)+sizeof(NetworkMessageHeader)];

		const Download::uuid_t REMOTE_INSTALLER_SYNC 
			={0x1b,0x1e,0xad,0x4a,0x46,0xeb,0x42,0xca,0xaa,0xb9,0xdc,0xdf,0x9b,0xea,0xde,0xcb};


		((NetworkMessageHeader*)xmessage) ->iLength
			 = 
			 (sizeof(NetworkMessageHeader)+sizeof(CommandInfo));

		((NetworkMessageHeader*)xmessage) ->iOffset=0;


		CommandInfo *pTemp = (CommandInfo *)(((char *)xmessage)+sizeof(NetworkMessageHeader));
	
		memcpy((char *)pTemp->TypeUUID, REMOTE_INSTALLER_SYNC, sizeof(uuid_t));


		struct sockaddr_in si_other;
		int s,  slen=sizeof(si_other);

		s=socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
				memset((char *) &si_other, 0, sizeof(si_other));
		si_other.sin_family = AF_INET;
		si_other.sin_port = htons(9930);
		si_other.sin_addr.s_addr =inet_addr("192.168.0.12");

		sendto(s, xmessage, sizeof(xmessage), 0, (sockaddr *)&si_other, slen);
	}

	return 0;

}

int VikingConsoleDriverImpl::isDeviceReady()
{
	return bDeviceExists_?1:0;
}

int VikingConsoleDriverImpl::setDownloadServerWorkingPath(wchar_t * path)
{
	wchar_t buffer[1024];
	SetCurrentDirectoryW(path);

    int dwRet = GetCurrentDirectoryW(sizeof(buffer), buffer);

    if(wcscmp(path,buffer) != 0)
    {
        return -1;
    }


	return 0;
}

int VikingConsoleDriverImpl::startDownloadServer()
{

    bStarted_=true;

    return 0;
}


int VikingConsoleDriverImpl::stopDownloadServer()
{
    bStarted_=false;
    return 0;
}


void VikingConsoleDriverImpl::reset()
{
	while(WaitForSingleObject(VikingConsoleDriverMutex_, 500 ) == WAIT_TIMEOUT)
	{
		if(bRunning_ == false)
		{
			break;
		}
		Sleep(5);
	}


	if(bRunning_)
	{
		rmtOps->reset();
		tokenList_.clear();


		CloseHandle(VikingConsoleDriverSemaphore_);

		VikingConsoleDriverSemaphore_ = CreateSemaphore(NULL,65535,65535,L"VikingConsoleDriverSemaphore");
		//TODO Find the best way to reset the semaphore or use boost::
		//Reset Semephore   ReleaseSemaphore (pVikingConsoleDriverImpl->VikingConsoleDriverSemaphore_,1,NULL);
	}

	bListenConfig_ = false;

	ReleaseMutex(VikingConsoleDriverMutex_);
}


ITokenImpl *  VikingConsoleDriverImpl::getNextToken(unsigned long msec)
{

    ITokenImpl * retVal=0;


    msec=max(msec,500);

    //There are no tokens avalable if not running
    if(bRunning_ == false)
    {
        return 0;
    }

    DWORD dwWaitResult = WaitForSingleObject( VikingConsoleDriverSemaphore_, msec);

    if( (dwWaitResult !=  WAIT_OBJECT_0) || (bRunning_ == false))
    {
        return 0;
    }

    while(WaitForSingleObject(VikingConsoleDriverMutex_, 500 ) == WAIT_TIMEOUT)
    {
        if(bRunning_ == false)
        {
            break;
        }
    }

    if(bRunning_ && !tokenList_.empty())
    {
        current_token=tokenList_.front();
        tokenList_.pop_front();

        retVal=current_token.get();
    }

    ReleaseMutex(VikingConsoleDriverMutex_);

    return retVal;
}
