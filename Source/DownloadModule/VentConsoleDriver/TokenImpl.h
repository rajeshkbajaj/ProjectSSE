#ifndef TOKENIMPL_H
#define TOKENIMPL_H
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


#include "ITokenImpl.h"

#include <string>
#include <vector>


class TokenImpl:public ITokenImpl
{
   private:
	   std::vector<std::string> word;

    public:
		void insertValue(std::string pIn);
        virtual const char *getValue(unsigned int r);
        virtual const int getLength();
 
};

#endif