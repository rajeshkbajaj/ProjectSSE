#ifndef REMOTEFILEOPERATIONS_H
#define REMOTEFILEOPERATIONS_H
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

#include"IRemoteFileOperations.h"
#include<string>
#include<memory>
#include<map>



class RemoteFileOperations:public IRemoteFileOperations
{
    private:

        bool bDebug_;
        std::string prepend_;
        std::string strip_;
        std::string rootdir_;
        std::string workingdir_;
		FILE* fp1;

        std::map<FILE *,std::string> openFiles_;
		bool isOpenDescriptor(FILE *fp);
		bool eraseOpenFile(char  *name);

        static int OnEverything(void *in,char *str, int len);


        int (*OnStderrWrite_)   (void *,char *,int len);
        int (*OnStdoutWrite_)   (void *,char *,int len);
        int (*OnFileOpen_)      (void *,char *,int len);
        int (*OnFileClose_)     (void *,char *,int len);
        int (*OnFileError_)     (void *,char *,int len);
        int (*OnSocketError_)   (void *,char *,int len);
        int (*OnResetTimeout_)  (void *,char *,int len);
        int (*OnReply_)         (void *,char *,int len);

        void *objStderrWrite_;
        void *objStdoutWrite_;
        void *objFileOpen_;
        void *objFileClose_;
        void *objFileError_;
        void *objSocketError_;
        void *objResetTimeout_;
        void *objReply_;


        int cov_parse_arglist(char *buffer, void **pVoidPointerArglist, 
                int *pSizeList, unsigned int *pTypeList, int size);
        int type_cmp(const unsigned int *s1, const unsigned int *s2);
        const char *strip(const char *ps1, const char *s2);

        std::tr1::shared_ptr<const char> prepend(const char *s1, const char *s2);


        int do_fopen(int argc, void **arglist,  int *lengthList,
                unsigned int *typeList, char *result_buffer,
                unsigned int *result_len);

        int do_fread(int argc, void **arglist,  int *lengthList,
                unsigned int *typeList, char *result_buffer,
                unsigned int *result_len);

        int do_fwrite(int argc, void **arglist,  int *lengthList,
                unsigned int *typeList, char *result_buffer,
                unsigned int *result_len);

        int do_fprintf(int argc, void **arglist, int *lengthList,
                unsigned int *typeList,  char *result_buffer,
                unsigned int *result_len);

        int do_fclose(int argc, void **arglist, int *lengthList,
                unsigned int *typeList, char *result_buffer,
                unsigned int *result_len);

        int do_fseek(int argc, void **arglist, int *lengthList,
                unsigned int *typeList,  char *result_buffer,
                unsigned int *result_len);

        int do_ftell(int argc, void **arglist,  int *lengthList,
                unsigned int *typeList, char *result_buffer,
                unsigned int *result_len);


    public:

        enum 
        {
            FN_OnStderrWrite_,
            FN_OnStdoutWrite_,
            FN_OnFileOpen_,
            FN_OnFileClose_,
            FN_OnFileError_,
            FN_OnSocketError_,
            FN_OnThreadExit_,
            FN_OnResetTimeout_,
            FN_OnReply_
        }; 

        virtual void reset();

        virtual void unregisterFn(unsigned int fnid);
        virtual void registerFn(unsigned int fnid, int (* callback)(void *, char *,int), void *obj);


        RemoteFileOperations();
        virtual ~RemoteFileOperations();

		virtual int  process_config(char *recv_buffer, unsigned int length);
        virtual int  process_msg(char *recv_buffer, unsigned int length);

};




#endif
