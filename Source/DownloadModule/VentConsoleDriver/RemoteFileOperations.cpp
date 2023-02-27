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

#include "RemoteFileOperations.h"
#include "file_io.h"
#include <Winsock2.h>

//-------------------------------------
// M A C R O S
//-------------------------------------
#ifndef min
#define min(X,Y)  (((X)<(Y))?(X):(Y))
#endif


unsigned char remote_fileio_head[16]       = {0x15,0x09,0x66,0x6f,0xcf,0xbe,0x4a,0x2c,0x86,0xdd,0x55,0x59,0xee,0xe1,0xb3,0xe6};
unsigned char remote_fileio_serverhead[16] = {0xad,0xa8,0xad,0x95,0xac,0xdf,0x4f,0x68,0x9d,0xea,0x72,0xa3,0x36,0xfd,0xbe,0xf6};


int RemoteFileOperations::OnEverything(void *in,char *str, int len)
{
    return 0;
}


int RemoteFileOperations::cov_parse_arglist(char *buffer, void **pVoidPointerArglist, 
        int *pSizeList, unsigned int *pTypeList, int size)
{

    unsigned int iHeadLength=0;
    unsigned int pindex = 0;
    unsigned int argcnt = 0;
    unsigned int iLength = 0;
    unsigned int type;


    while (size > 0)
    {
        type = ntohl(((ByteMessage *) (&buffer[pindex]))->type);

        if(pTypeList)
        {
            pTypeList[argcnt]=type;
        }

        switch (type) {
            case TYPEID_BYTE:
            case TYPEID_CHARSZ:
                pVoidPointerArglist[argcnt] = ((ByteMessage *) (&buffer[pindex]))->value;
                iLength = ntohl(((ByteMessage *) (&buffer[pindex]))->length);
                iHeadLength=sizeof(ByteMessage);

                ///Check if string is terminated
                if(type == TYPEID_CHARSZ)
                {
                    if( buffer[pindex-1+iLength]!='\0'  )
                    {
                        std::tr1::shared_ptr<char> ptr(new char [300]);
                        sprintf(ptr.get(),"Received string argument not terminated.");
                        OnFileError_(objFileError_,(char *)(ptr.get()), strlen((ptr.get()))+1);
                        return -1;
                    }
                }
                break;
            case TYPEID_RAWBYTE:
                pVoidPointerArglist[argcnt] = ((ByteMessage *) (&buffer[pindex]))->value;
                iLength = ntohl(((ByteMessage *) (&buffer[pindex]))->length);
                iHeadLength=sizeof(ByteMessage);

                break;
            case TYPEID_INT:
            case TYPEID_MODET:
            case TYPEID_SIZET:
            case TYPEID_OFFT:
            case TYPEID_FILET:
                pVoidPointerArglist[argcnt] =
                    (void *)ntohl(((IntMessage *) (&buffer[pindex]))->value);
                iLength = sizeof(IntMessage);
                iHeadLength=sizeof(IntMessage);

                break;

            default:
                std::tr1::shared_ptr<char> ptr(new char [300]);
                sprintf(ptr.get(),"Argument type unknown.");
                OnFileError_(objFileError_,(char *)(ptr.get()), strlen((ptr.get()))+1);
                return -1;
        }

        if(pSizeList)
        {
            pSizeList[argcnt]=iLength-iHeadLength;
        }

        argcnt++;
        size   -= iLength;
        pindex += iLength;
    }


    if(pTypeList)
    {
        pTypeList[argcnt]=0;
    }

    if(size!=0)
    {
        std::tr1::shared_ptr<char> ptr(new char [300]);
        sprintf(ptr.get(),"Number of arguments mismatch.");
        OnFileError_(objFileError_,(char *)(ptr.get()), strlen((ptr.get()))+1);
    }


    return (size==0)?(argcnt):(-1);
} 


bool RemoteFileOperations::isOpenDescriptor(FILE *fp)
{

	std::map<FILE *,std::string>::const_iterator it= openFiles_.find(fp);

    if(fp==stdout || fp==stderr || fp==stdin || (it!=openFiles_.end()))
	{
		return true;
	}

	return false;

}


bool RemoteFileOperations::eraseOpenFile(char  *name)
{

	std::map<FILE *,std::string>::const_iterator it= openFiles_.begin();

    for(;it!=openFiles_.end();it++)
	{
		if(strcmp(it->second.c_str(),name) == 0 )
		{
			fclose(it->first);
			openFiles_.erase(it);
			return true;
		}
	}

	return false;
}




int RemoteFileOperations::type_cmp(const unsigned int *s1, const unsigned int *s2)
{
    while ((*s1 != TYPEID_NIL) && (*s1 == *s2))
    {
        s1++, s2++;
    }

    return ((unsigned int)*s1) - ((unsigned int)*s2);
} 

const char *RemoteFileOperations::strip(const char *ps1, const char *s2){

    const char *s1=ps1;

    while ((*s1 != '\0') && (*s1 == *s2))
    {
        s1++, s2++;
    }

    if( *s1 != '\0' )
    {
        return ps1;
    }

    return s1;
} 


std::tr1::shared_ptr<const char> RemoteFileOperations::prepend(const char *s1, const char *s2){
    char *psz;
    int len=strlen(s1)+strlen(s2)+1;

    psz=new char[len];

    sprintf(psz,"%s%s",s1,s2);


    return std::tr1::shared_ptr<char>(psz);
} 



void RemoteFileOperations::reset()
{

	//TODO[ME]: Add mutex around openFiles iterator


    std::map<FILE *,std::string>::iterator it= openFiles_.begin();

    for(;it!=openFiles_.end();it++)
    {
        OnFileClose_(objFileClose_,(char *)it->second.c_str(),strlen(it->second.c_str()));

        fclose(it->first);
    }

	openFiles_.clear();

}

void RemoteFileOperations::unregisterFn(unsigned int fnid)
{
    switch(fnid)
    {
        case FN_OnStderrWrite_:
            OnStderrWrite_ = OnEverything;
            break;

        case FN_OnStdoutWrite_:
            OnStdoutWrite_ = OnEverything;
            break;

        case FN_OnFileOpen_:
            OnFileOpen_   = OnEverything;
            break;

        case FN_OnFileClose_:
            OnFileClose_  = OnEverything;
            break;

        case FN_OnFileError_:
            OnFileError_  = OnEverything;
            break;

        case FN_OnSocketError_:
            OnSocketError_ = OnEverything;
            break;

        case FN_OnResetTimeout_:
            OnResetTimeout_ = OnEverything;
            break;

        case FN_OnReply_:
            OnReply_ = OnEverything;
            break;
    }
}


void RemoteFileOperations::registerFn(unsigned int fnid, int (* callback)(void *, char *,int), void *obj)
{
    switch(fnid)
    {
        case FN_OnStderrWrite_:
            OnStderrWrite_ = callback;
            objStderrWrite_ = obj;
            break;

        case FN_OnStdoutWrite_:
            OnStdoutWrite_ = callback;
            objStdoutWrite_= obj;
            break;

        case FN_OnFileOpen_:
            OnFileOpen_   = callback;
            objFileOpen_  = obj;
            break;

        case FN_OnFileClose_:
            OnFileClose_  = callback;
            objFileClose_ = obj;
            break;

        case FN_OnFileError_:
            OnFileError_  = callback;
            objFileError_ = obj;
            break;

        case FN_OnSocketError_:
            OnSocketError_ = callback;
            objSocketError_= obj;
            break;

        case FN_OnResetTimeout_:
            OnResetTimeout_ = callback;
            objResetTimeout_= obj;
            break;

        case FN_OnReply_:
            OnReply_ = callback;
            objReply_= obj;
            break;
    }
}

RemoteFileOperations::RemoteFileOperations():bDebug_(false),prepend_(""),strip_(""),
    rootdir_(""),
    workingdir_(""),
    openFiles_(),
    OnStderrWrite_(OnEverything),
    OnStdoutWrite_(OnEverything),
    OnFileOpen_   (OnEverything),
    OnFileClose_  (OnEverything),
    OnFileError_  (OnEverything),
    OnSocketError_(OnEverything),
    OnResetTimeout_(OnEverything),
    OnReply_(OnEverything)

{
#define GEN_DEBUG_FILE
#ifdef GEN_DEBUG_FILE
	fp1=fopen("OUTPUT.txt","wb");
#else
	fp1 = NULL;
#endif
}

RemoteFileOperations::~RemoteFileOperations()
{
	if(fp1 != NULL)
	{
		fclose(fp1);
	}

}


int RemoteFileOperations::do_fopen(int argc, void **arglist,  int *lengthList,
        unsigned int *typeList, char *result_buffer,
        unsigned int *result_len)
{


    FILE *fp = 0;
    unsigned int bindex = 0;
    unsigned int len;

    static const unsigned int arg_def[]={TYPEID_CHARSZ,TYPEID_CHARSZ,TYPEID_NIL};

    /*Check number of arguments */
    if(argc!=2)
    {
        return COV_WRONG_NUMBER_ARGS;
    }
    /*Make sure type compatable*/
    if(type_cmp(arg_def,typeList)!=0)
    {
        return COV_ARG_TYPE_MISMATCH;
    }

	char *str= (char * )arglist[0];

	for(int i=0;str[i]!='\0';i++)
	{
		if(str[i]=='/')
		{
			str[i]='\\';
		}
	}


   std::tr1::shared_ptr<const char> fname1(_strdup(str));
     //   prepend(prepend_.c_str(),strip((char * )arglist[0],strip_.c_str()));

    
    eraseOpenFile((char *)fname1.get());

	//try three times; Virus checker locks file...
	for(int i=0;i<3;i++)
	{
		fp = fopen(fname1.get(), (const char *)arglist[1]);

		if(fp)
		{
			break;
		}
		Sleep(10);
	}

    if(fp)
    {

        openFiles_.insert(std::pair<FILE *, std::string>(fp,fname1.get()));
        OnFileOpen_(objFileOpen_,(char *)(fname1.get()), strlen((fname1.get()))+1);


		       
        fprintf(stderr,">>>>>>>Opened FIle, \"%s\" %x.",fname1.get(),fp);
       
    }
    else
    {
        std::tr1::shared_ptr<char> ptr(new char [300]);
        sprintf(ptr.get(),">>>>>>>Could not open file, \"%s\".",fname1.get());
        OnFileError_(objFileError_,(char *)(ptr.get()), strlen((ptr.get()))+1);
    }


    /*Build return header */
    ((CallMessage *) (&result_buffer[bindex]))->type = htonl(COV_FOPEN);
    bindex = sizeof(CallMessage);

    /*Add return pointer address */
    len = ((sizeof(IntMessage)) + 0x3) & ~0x3;
    ((IntMessage *) (&result_buffer[bindex]))->type  = htonl(TYPEID_FILET);
    ((IntMessage *) (&result_buffer[bindex]))->value = htonl((unsigned int)fp);
    bindex += len;

    /*Set length */
    ((CallMessage *) (&result_buffer[0]))->length = htonl(bindex);
    (*result_len) = bindex;


    return 0;
} // do_fopen


int RemoteFileOperations::do_fread(int argc, void **arglist,  int *lengthList,
        unsigned int *typeList, char *result_buffer,
        unsigned int *result_len)
{

    char ret_buffer[2000];
    int retval;

    unsigned int bindex = 0;
    unsigned int len;

    static const unsigned int arg_def[]={TYPEID_SIZET,TYPEID_SIZET,TYPEID_FILET,TYPEID_NIL};

    /*Check number of arguments */
    if(argc!=3)
    {
        return COV_WRONG_NUMBER_ARGS;
    }
    /*Make sure type compatable*/
    if(type_cmp(arg_def,typeList)!=0)
    {
        return COV_ARG_TYPE_MISMATCH;
    }

    FILE *fp=(FILE *)arglist[2];

    if(((unsigned int)fp)==0)
    {
        fp=stdin;
    }
    else if(((unsigned int)fp)==1)
    {
        fp=stdout;
    }
    else if(((unsigned int)fp)==2)
    {
        fp=stderr;
    }


    if(isOpenDescriptor(fp))
    {

        retval = fread(ret_buffer, (size_t) arglist[0], (size_t) arglist[1],
                (FILE *) fp);
    }
    else
    {
        OnFileError_(objFileError_,"Bad file descriptor used in call to fread.",43);
        retval = -1;
    }


    /*Build return header */
    ((CallMessage *) (&result_buffer[bindex]))->type = htonl(FREAD);
    bindex = sizeof(CallMessage);

    /*Add return result */
    len = ((sizeof(IntMessage)) + 0x3) & ~0x3;
    ((IntMessage *) (&result_buffer[bindex]))->type = htonl(TYPEID_INT);
    ((IntMessage *) (&result_buffer[bindex]))->value = htonl(retval);
    bindex += len;

    /*Add return buffer */
    len = (((retval * (int)arglist[0]) + sizeof(ByteMessage)) + 0x3) & ~0x3;
    ((ByteMessage *) (&result_buffer[bindex]))->type = htonl(TYPEID_RAWBYTE);
    ((ByteMessage *) (&result_buffer[bindex]))->length = htonl(len);
    if(retval != -1)
    {
        memcpy(((ByteMessage *) (&result_buffer[bindex]))->value,
                (ret_buffer), (retval * (int)arglist[0]));
    }
    bindex += len;

    /*Set length */
    ((CallMessage *) (&result_buffer[0]))->length = htonl(bindex);
    (*result_len) = bindex;

    return 0;

} // do_fread


int RemoteFileOperations::do_fwrite(int argc, void **arglist,  int *lengthList,
        unsigned int *typeList, char *result_buffer,
        unsigned int *result_len)
{
    int retval;

    unsigned int bindex = 0;
    unsigned int len;

    static const unsigned int arg_def[]={TYPEID_RAWBYTE,TYPEID_SIZET,TYPEID_SIZET,TYPEID_FILET,TYPEID_NIL};

    /*Check number of arguments */
    if(argc!=4)
    {
        return COV_WRONG_NUMBER_ARGS;
    }
    /*Make sure type compatable*/
    if(type_cmp(arg_def,typeList)!=0)
    {
        return COV_ARG_TYPE_MISMATCH;
    }
    FILE *fp=(FILE *)arglist[3];

    if(((unsigned int)fp)==0)
    {
        fp=stdin;
    }
    else if(((unsigned int)fp)==1)
    {
#ifdef GEN_DEBUG_FILE
		fwrite((char *)arglist[0],(size_t) arglist[1], ((size_t) arglist[2]) -1, fp1);
			fflush(fp1);
#endif
        OnStdoutWrite_(objStdoutWrite_,(char *)arglist[0],((size_t) arglist[1]) * ((size_t) arglist[2]));
		fp=stdout;
    }
    else if(((unsigned int)fp)==2)
    {
#ifdef GEN_DEBUG_FILE
		fwrite((char *)arglist[0],(size_t) arglist[1], ((size_t) arglist[2])-1, fp1);
		fflush(fp1);
#endif

        OnStderrWrite_(objStderrWrite_,(char *)arglist[0],((size_t) arglist[1]) * ((size_t) arglist[2]));
        fp=stderr;
    }



    if(isOpenDescriptor(fp)&&fp!=stderr &&fp!=stdin)
    {
		
        retval = fwrite((char *)arglist[0], (size_t) arglist[1], (size_t) arglist[2],
                (FILE *) fp);
    }
    else
    {
        OnFileError_(objFileError_,"Bad file descriptor used in call to fwrite.", 44);
        retval = 0;
    }


    /*Build return header */
    ((CallMessage *) (&result_buffer[bindex]))->type = htonl(FWRITE);
    bindex = sizeof(CallMessage);

    /*Add return result */
    len = ((sizeof(IntMessage)) + 0x3) & ~0x3;
    ((IntMessage *) (&result_buffer[bindex]))->type = htonl(TYPEID_INT);
    ((IntMessage *) (&result_buffer[bindex]))->value = htonl(retval);
    bindex += len;

    /*Set length */
    ((CallMessage *) (&result_buffer[0]))->length = htonl(bindex);
    (*result_len) = bindex;
    return 0;
} // do_fwrite


int RemoteFileOperations::do_fprintf(int argc, void **arglist, int *lengthList,
        unsigned int *typeList,  char *result_buffer,
        unsigned int *result_len)
{
    int retval = 0;

    unsigned int bindex = 0;
    unsigned int len;


    static const unsigned int arg_def[]={TYPEID_FILET,TYPEID_CHARSZ,TYPEID_NIL};

    /*Check number of arguments */
    if(argc!=2)
    {
        return COV_WRONG_NUMBER_ARGS;
    }


    /*Make sure type compatable*/
    if(type_cmp(arg_def,typeList)!=0)
    {
        return COV_ARG_TYPE_MISMATCH;
    }

    FILE *fp=(FILE *)arglist[0];

    if(((unsigned int)fp)==0)
    {
        fp=stdin;
    }
    else if(((unsigned int)fp)==1)
    {
        OnStdoutWrite_(objStdoutWrite_,(char *)arglist[1],strlen((char *)arglist[1])+1);
        fp=stdout;
    }
    else if(((unsigned int)fp)==2)
    {
        OnStderrWrite_(objStdoutWrite_,(char *)arglist[1],strlen((char *)arglist[1])+1);
        fp=stderr;
    }




    if(isOpenDescriptor(fp))
    {
        //retval = fprintf(fp, "%s",(const char *)arglist[1]);
    }
    else
    {
        OnFileError_(objFileError_,"Bad file descriptor used in call to fprintf.",45);
    }


    /*Build return header */
    ((CallMessage *) (&result_buffer[bindex]))->type = htonl(FWRITE);
    bindex = sizeof(CallMessage);

    /*Add return result */
    len = ((sizeof(IntMessage)) + 0x3) & ~0x3;
    ((IntMessage *) (&result_buffer[bindex]))->type = htonl(TYPEID_INT);
    ((IntMessage *) (&result_buffer[bindex]))->value = htonl(retval);
    bindex += len;

    /*Set length */
    ((CallMessage *) (&result_buffer[0]))->length = htonl(bindex);
    (*result_len) = bindex;

    return 0;
} // do_fprintf


int RemoteFileOperations::do_fclose(int argc, void **arglist, int *lengthList,
        unsigned int *typeList, char *result_buffer,
        unsigned int *result_len)
{
    int retval;
    unsigned int bindex = 0;
    unsigned int len;


    static const unsigned int arg_def[]={TYPEID_FILET,TYPEID_NIL};

    /*Check number of arguments */
    if(argc!=1)
    {
        return COV_WRONG_NUMBER_ARGS;
    }
    /*Make sure type compatable*/
    if(type_cmp(arg_def,typeList)!=0)
    {
        return COV_ARG_TYPE_MISMATCH;
    }


    FILE *fp=(FILE *)arglist[0];

    if(((unsigned int)fp)==0)
    {
        fp=stdin;
    }
    else if(((unsigned int)fp)==1)
    {
        fp=stdout;
    }
    else if(((unsigned int)fp)==2)
    {
        fp=stderr;
    }



    if(isOpenDescriptor(fp) && fp !=stderr && fp !=stdout && fp !=stdin)
    {

        std::map<FILE *, std::string>::iterator it = openFiles_.find(fp);

        if(it!=openFiles_.end())
        {
            OnFileClose_(objFileClose_,(char *)it->second.c_str(),strlen(it->second.c_str()));
        }
        else
        {
            OnFileClose_(objFileClose_,"",1);
        }

        retval = fclose((FILE *)fp);

        openFiles_.erase((FILE *)fp);

    }
    else
    {
        OnFileError_(objFileError_,"Bad file descriptor used in call to fclose.",44);
        retval=EOF;
    }


    /*Build return header */
    ((CallMessage *) (&result_buffer[bindex]))->type = htonl(FWRITE);
    bindex = sizeof(CallMessage);

    /*Add return result */
    len = ((sizeof(IntMessage)) + 0x3) & ~0x3;
    ((IntMessage *) (&result_buffer[bindex]))->type = htonl(TYPEID_INT);
    ((IntMessage *) (&result_buffer[bindex]))->value = htonl(retval);
    bindex += len;

    /*Set length */
    ((CallMessage *) (&result_buffer[0]))->length = htonl(bindex);
    (*result_len) = bindex;
    return 0;
} // do_fclose


int RemoteFileOperations::do_fseek(int argc, void **arglist, int *lengthList,
        unsigned int *typeList,  char *result_buffer,
        unsigned int *result_len)
{
    int retval;
    unsigned int bindex = 0;
    unsigned int len;


    static const unsigned int arg_def[]={TYPEID_FILET,TYPEID_INT,TYPEID_INT,TYPEID_NIL};

    /*Check number of arguments */
    if(argc != 3){
        return COV_WRONG_NUMBER_ARGS;
    }
    /*Make sure type compatable*/
    if(type_cmp(arg_def,typeList) != 0 )
    {
        return COV_ARG_TYPE_MISMATCH;
    }

    if(isOpenDescriptor((FILE *)arglist[0]))
    {
        retval = fseek((FILE *) arglist[0], (long)arglist[1], (int)arglist[2]);
    }
    else
    {
        OnFileError_(objFileError_,"Bad file descriptor used in call to fseek.",43);
        retval = -1;
    }

    /*Build return header */
    ((CallMessage *) (&result_buffer[bindex]))->type = htonl(FSEEK);
    bindex = sizeof(CallMessage);

    /*Add return result */
    len = ((sizeof(IntMessage)) + 0x3) & ~0x3;
    ((IntMessage *) (&result_buffer[bindex]))->type = htonl(TYPEID_INT);
    ((IntMessage *) (&result_buffer[bindex]))->value = htonl(retval);
    bindex += len;

    /*Set length */
    ((CallMessage *) (&result_buffer[0]))->length = htonl(bindex);
    (*result_len) = bindex;

    return 0;
} // do_fseek


int RemoteFileOperations::do_ftell(int argc, void **arglist,  int *lengthList,
        unsigned int *typeList, char *result_buffer, unsigned int *result_len)
{

    int retval;
    unsigned int bindex = 0;
    unsigned int len;

    static const unsigned int arg_def[]={TYPEID_FILET,TYPEID_NIL};

    /*Check number of arguments */
    if(argc!=1)
    {
        return COV_WRONG_NUMBER_ARGS;
    }

    /*Make sure type compatable*/
    if(type_cmp(arg_def,typeList)!=0)
    {
        return COV_ARG_TYPE_MISMATCH;
    }

    if(isOpenDescriptor((FILE *)arglist[0]))
    {
        retval = ftell((FILE *)arglist[0]);
    }
    else
    {
        OnFileError_(objFileError_,"Bad file descriptor used in call to ftell.",43);
        retval = -1;
    }


    /*Build return header */
    ((CallMessage *) (&result_buffer[bindex]))->type = htonl(FTELL);
    bindex = sizeof(CallMessage);

    /*Add return result */
    len = ((sizeof(IntMessage)) + 0x3) & ~0x3;
    ((IntMessage *) (&result_buffer[bindex]))->type = htonl(TYPEID_INT);
    ((IntMessage *) (&result_buffer[bindex]))->value = htonl(retval);
    bindex += len;

    /*Set length */
    ((CallMessage *) (&result_buffer[0]))->length = htonl(bindex);
    (*result_len) = bindex;

    return 0;
} // do_ftell

int RemoteFileOperations::process_config(char *recv_buffer, unsigned int pck_length)
{
	unsigned int result_len=0;
    unsigned int typeList[20];
    int lengthList[20];
    void *arglist[20];
    int iRetval;



    int type =   ntohl(((CallMessage *) recv_buffer)->type);
    unsigned int length = ntohl(((CallMessage *) recv_buffer)->length);

    if(pck_length < length)
    {
        std::tr1::shared_ptr<char> ptr(new char [300]);
        sprintf(ptr.get(),"Failed to parse command (Invalid length).");
        OnFileError_(objFileError_,(char *)(ptr.get()), strlen((ptr.get()))+1);
        return -1;
    }

    std::tr1::shared_ptr<char> result_buffer(new char [3000]);
    memset( result_buffer.get(),0,3000);

    int argc = cov_parse_arglist(recv_buffer + sizeof(CallMessage), arglist,lengthList, typeList,
            length - sizeof(CallMessage));


    if(argc < 0)
    {
        std::tr1::shared_ptr<char> ptr(new char [300]);
        sprintf(ptr.get(),"Failed to parse command.");
        OnFileError_(objFileError_,(char *)(ptr.get()), strlen((ptr.get()))+1);
        return -1;
    }

	switch (type)
    {
        case FWRITE:
            iRetval=do_fwrite(argc, arglist,lengthList, typeList,  result_buffer.get(), &result_len);
            break;
        case FPRINTF:
            iRetval=do_fprintf(argc, arglist, lengthList, typeList, result_buffer.get(), &result_len);
            break;
		default:
			iRetval = -1;
			break;
	}

	return iRetval;
}

int RemoteFileOperations::process_msg(char *recv_buffer, unsigned int pck_length)
{

    unsigned int result_len=0;
    unsigned int typeList[20];
    int lengthList[20];
    void *arglist[20];
    int iRetval;



    int type =   ntohl(((CallMessage *) recv_buffer)->type);
    unsigned int length = ntohl(((CallMessage *) recv_buffer)->length);

    if(pck_length < length)
    {
        std::tr1::shared_ptr<char> ptr(new char [300]);
        sprintf(ptr.get(),"Failed to parse command (Invalid length).");
        OnFileError_(objFileError_,(char *)(ptr.get()), strlen((ptr.get()))+1);
        return -1;
    }

    std::tr1::shared_ptr<char> result_buffer(new char [3000]);
    memset( result_buffer.get(),0,3000);

    int argc = cov_parse_arglist(recv_buffer + sizeof(CallMessage), arglist,lengthList, typeList,
            length - sizeof(CallMessage));


    if(argc < 0)
    {
        std::tr1::shared_ptr<char> ptr(new char [300]);
        sprintf(ptr.get(),"Failed to parse command.");
        OnFileError_(objFileError_,(char *)(ptr.get()), strlen((ptr.get()))+1);
        return -1;
    }


    switch (type)
    {
        case COV_FOPEN:
            iRetval=do_fopen(argc, arglist,lengthList, typeList, result_buffer.get(), &result_len);
            break;

        case FREAD:
            iRetval=do_fread(argc, arglist,lengthList, typeList,  result_buffer.get(), &result_len);
            break;

        case FWRITE:
            iRetval=do_fwrite(argc, arglist,lengthList, typeList,  result_buffer.get(), &result_len);
            break;

        case FCLOSE:
            iRetval=do_fclose(argc, arglist, lengthList, typeList, result_buffer.get(), &result_len);
            break;

        case FSEEK:
            iRetval=do_fseek(argc, arglist, lengthList, typeList, result_buffer.get(), &result_len);
            break;

        case FTELL:
            iRetval=do_ftell(argc, arglist, lengthList, typeList, result_buffer.get(), &result_len);
            break;

        case FPRINTF:
            iRetval=do_fprintf(argc, arglist, lengthList, typeList, result_buffer.get(), &result_len);
            break;

        default:
            std::tr1::shared_ptr<char> ptr(new char [300]);
            sprintf(ptr.get(),"Unknown command with id=%x\n",type);
            OnFileError_(objFileError_,(char *)(ptr.get()), strlen((ptr.get()))+1);
            return -1;
    }



	memcpy((((CallMessage *) (result_buffer.get()))->remote_fileio_head),
		remote_fileio_serverhead,sizeof(remote_fileio_serverhead));



    if(iRetval == 0)
    {
        OnResetTimeout_(this->objResetTimeout_,"",1);
        return OnReply_(objReply_, result_buffer.get(), result_len);
    }

    char *pErrStr = "";
    switch(iRetval)
    {
        case COV_WRONG_NUMBER_ARGS :
            pErrStr="Wrong number of arguments\n";
            break;
        case COV_ARG_TYPE_MISMATCH :
            pErrStr="Argument type mismatch\n";
            break;
        case COV_DATA_SIZE_MISMATCH:
            pErrStr="Argument data size mismatch\n";
            break;
    }

    OnFileError_(objFileError_,pErrStr, strlen(pErrStr)+1);

    return iRetval;
}
