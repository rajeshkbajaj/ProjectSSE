#ifndef IVIKINGCONSOLEDRIVERIMPL_H
#define IVIKINGCONSOLEDRIVERIMPL_H
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

class ITokenImpl;
class IRemoteFileOperations;


class IVikingConsoleDriverImpl
{
    public:
        virtual ITokenImpl *  getNextToken(unsigned long msec = 0) = 0;
        virtual void finish() = 0;
        virtual void reset() = 0;
        virtual int startDownloadServer() = 0;
        virtual int stopDownloadServer() = 0;
		virtual int getDownloadServerState() = 0;

        virtual void setDelay(int i) = 0;
        virtual ~IVikingConsoleDriverImpl(){}
		virtual int enableTrigger() = 0;
		virtual int disableTrigger() = 0;
		virtual int getTriggerState() = 0;
		virtual int oneShotTrigger() = 0;
		virtual int isDeviceReady() = 0;
		virtual void listenForConfig() = 0;

		virtual int setDownloadServerWorkingPath(wchar_t * path) =0;
};

#endif
