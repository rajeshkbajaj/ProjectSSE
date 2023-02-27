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

#include "TokenImpl.h"


void TokenImpl::insertValue(std::string pIn)
{
    word.push_back(pIn);
}


const char *TokenImpl::getValue(unsigned int r)
{

    if ( (r >= this->word.size()) || 
            (this->word.size()==0) )
    {
        return "";
    }
    else
    {
        return this->word.at(r).c_str();
    }
}

const int TokenImpl::getLength()
{
    return this->word.size();
}
