﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WcfWebService
{
    //To store info of one blob
    [DataContract]
    public class Blob  
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Uri { get; set; }
    }
   
}