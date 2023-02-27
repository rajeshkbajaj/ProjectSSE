#ifndef IREMOTEFILEOPERATIONS_H
#define IREMOTEFILEOPERATIONS_H
//----------------------------------------------------------------------------
//            Copyright (c) 2011 Covidien, Inc.
//
// This software is copyrighted by and is the sole property of Covidien. This
// is a proprietary work to which Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Covidien.
//----------------------------------------------------------------------------



class IRemoteFileOperations
{
    public:
        virtual void reset() = 0;

        virtual void unregisterFn(unsigned int fnid) = 0;
        virtual void registerFn(unsigned int fnid,
                int (* callback)(void *, char *,int), void *obj) = 0;

		virtual int  process_config(char *recv_buffer, unsigned int length) = 0;
        virtual int  process_msg(char *recv_buffer, unsigned int length) = 0;

        ~IRemoteFileOperations(){}
};

#endif
