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
using System.Collections.Generic;

namespace PB980Mock.WebServices
{
    internal class GetDeviceInformationAsXml : WebPage
    {
        public GetDeviceInformationAsXml(string pname)
            : base(pname)
        {
        }

        public override string ExecuteRequest(Dictionary<string, string> arguments)
        {

            return string.Format(@"<?xml version='1.0' standalone='yes' ?>
<body>
   <device_identity>
      <make>Puritan Bennett</make>
      <modle>PB980</modle>
      <type>Ventilator</type>
      <serial_number>35B12P3001</serial_number>
      <release>10097144X00</release>
   </device_identity>
   <components>
      <component type='SOFTWARE'>
         <name>PB.BD.FLASH.BOOT</name>
         <part_number>10099442</part_number>
         <revision>X00</revision>
         <md5>d00112b0ff6d8ce4b99cb5635be37864</md5>
      </component>
      <component type='SOFTWARE'>
         <name>PB.BD.FLASH.DOWNLOAD</name>
         <part_number>10099442</part_number>
         <revision>X00</revision>
         <md5>0ed1f72fbb440186d52b2efc3928880c</md5>
      </component>
      <component type='SOFTWARE'>
         <name>PB.BD.FLASH.NORMAL</name>
         <part_number>10097144</part_number>
         <revision>X00</revision>
         <md5>ecb3ab7b6c66fb91f979f1f64fb651b5</md5>
      </component>
      <component type='SOFTWARE'>
         <name>PB.GUI.FLASH.BOOT</name>
         <part_number>10099442</part_number>
         <revision>X00</revision>
         <md5>d00112b0ff6d8ce4b99cb5635be37864</md5>
      </component>
      <component type='SOFTWARE'>
         <name>PB.GUI.FLASH.DOWNLOAD</name>
         <part_number>10099442</part_number>
         <revision>X00</revision>
         <md5>0ed1f72fbb440186d52b2efc3928880c</md5>
      </component>
      <component type='SOFTWARE'>
         <name>PB.GUI.FLASH.SERVICE</name>
         <part_number>10097144</part_number>
         <revision>X00</revision>
         <md5>1611e53649feb7d6ff99068fe11bd275</md5>
      </component>
      <component type='SOFTWARE'>
         <name>PB.GUI.FLASH.NORMAL</name>
         <part_number>10097144</part_number>
         <revision>X00</revision>
         <md5>34dabbe613d65b352d3cfe02577cf120</md5>
      </component>
      <component type='HARDWARE'>
         <name>MasterVent</name>
         <serial_number>35B12P3001</serial_number>
         <operational_hours>800</operational_hours>
         <options>ATC;NEO_MODE;VTPC;RESP_MECH;TRENDING;NEO_MODE_UPDATE;NEO_MODE_ADVANCED;BILEVEL;PAV;IE_SYNC_ASYNC;LEAK_COMP;SERVICE_KEY;</options>
         <datakey_type>ENGINEERING</datakey_type>
         <pm_hours_remaining>352</pm_hours_remaining>
         <cal_hours_remaining>0</cal_hours_remaining>
         <est_hours_remaining>0</est_hours_remaining>
      </component>
      <component type='SOFTWARE'>
         <name>BdSoftware</name>
         <revision>VIKING_3.1.3.191</revision>
      </component>
      <component type='HARDWARE'>
         <name>InspFlowSensor</name>
         <part_number>840610</part_number>
         <serial_number>6101233041</serial_number>
         <revision>56</revision>
      </component>
      <component type='HARDWARE'>
         <name>ExhFlowSensor</name>
         <part_number>840600</part_number>
         <serial_number>6001234007</serial_number>
         <revision>57</revision>
      </component>
      <component type='HARDWARE'>
         <name>MixAirFlowSensor</name>
         <part_number>840610</part_number>
         <serial_number>6101233040</serial_number>
         <revision>56</revision>
      </component>
      <component type='HARDWARE'>
         <name>MixO2FlowSensor</name>
         <part_number>840610</part_number>
         <serial_number>6101219275</serial_number>
         <revision>56</revision>
      </component>
      <component type='HARDWARE'>
         <name>PowerCompAgileMotorController</name>
         <part_number>Initialized Value</part_number>
         <serial_number>Initialize</serial_number>
         <revision>Initialized Value</revision>
      </component>
      <component type='HARDWARE'>
         <name>PiInspPcbaBoard</name>
         <part_number></part_number>
         <serial_number></serial_number>
         <revision></revision>
      </component>
      <component type='HARDWARE'>
         <name>PiInspIfmPcbaBoard</name>
         <part_number>10046269</part_number>
      </component>
      <component type='HARDWARE'>
         <name>ExhSensorPcbaBoard</name>
         <part_number>10046191</part_number>
      </component>
      <component type='HARDWARE'>
         <name>PiExhValvePcbaBoard</name>
         <part_number>10046187</part_number>
      </component>
      <component type='HARDWARE'>
         <name>MixPcbaBoard</name>
         <part_number></part_number>
         <serial_number></serial_number>
         <revision></revision>
      </component>
      <component type='HARDWARE'>
         <name>PowerVentPcbaBoard</name>
         <part_number></part_number>
         <serial_number></serial_number>
         <revision></revision>
      </component>
      <component type='HARDWARE'>
         <name>PowerCompControllerPcbaBoard</name>
         <part_number>Not Avail</part_number>
         <serial_number>Not Avail</serial_number>
         <revision>Not Avail</revision>
      </component>
      <component type='HARDWARE'>
         <name>PowerCompInterfacePcbaBoard</name>
         <part_number>Not Avail</part_number>
         <serial_number>Not Avail</serial_number>
         <revision>Not Avail</revision>
      </component>
      <component type='HARDWARE'>
         <name>Options1PcbaBoard</name>
         <part_number>Initialized Value</part_number>
         <serial_number>Initialize</serial_number>
         <revision>Initialized Value</revision>
      </component>
      <component type='HARDWARE'>
         <name>UiPcbaBoard</name>
         <serial_number>B</serial_number>
      </component>
      <component type='FIRMWARE'>
         <name>CpldBd</name>
         <part_number>10089860</part_number>
         <revision>X05.50</revision>
      </component>
      <component type='FIRMWARE'>
         <name>CpldGui</name>
         <part_number>10089862</part_number>
         <revision>X06.27</revision>
      </component>
      <component type='FIRMWARE'>
         <name>FpgaBd</name>
         <part_number>10089852</part_number>
         <revision>X04.30</revision>
      </component>
      <component type='FIRMWARE'>
         <name>FpgaGui</name>
         <part_number>10089726</part_number>
         <revision>X03.40</revision>
      </component>
      <component type='FIRMWARE'>
         <name>FpgaInspiratory</name>
         <part_number>10089854</part_number>
         <revision>X06.57</revision>
      </component>
      <component type='FIRMWARE'>
         <name>FpgaExhalation</name>
         <part_number>10089854</part_number>
         <revision>X06.57</revision>
      </component>
      <component type='FIRMWARE'>
         <name>FpgaMix</name>
         <part_number>10089992</part_number>
         <revision>X07.13</revision>
      </component>
      <component type='FIRMWARE'>
         <name>FpgaPowerControl</name>
         <part_number>10089856</part_number>
         <revision>X05.42</revision>
      </component>
      <component type='FIRMWARE'>
         <name>FpgaOptionAccessories</name>
         <part_number>Not Installed</part_number>
         <revision>Not Installed</revision>
      </component>
      <component type='FIRMWARE'>
         <name>FpgaUi</name>
         <part_number>10089858</part_number>
         <revision>X04.41</revision>
      </component>
      <component type='FIRMWARE'>
         <name>UiAudioFlash</name>
         <part_number>10089997</part_number>
         <revision>X01.28</revision>
      </component>
      <component type='HARDWARE'>
         <name>PrimaryVentBattery</name>
         <serial_number>0</serial_number>
      </component>
      <component type='HARDWARE'>
         <name>ExtendedVentBattery</name>
         <serial_number>0</serial_number>
      </component>
      <component type='HARDWARE'>
         <name>PrimaryCompressorBattery</name>
         <serial_number>0</serial_number>
      </component>
      <component type='HARDWARE'>
         <name>BdPcba</name>
         <serial_number>XXXXXXXXXXXXX</serial_number>
      </component>
      <component type='HARDWARE'>
         <name>GuiPcba</name>
         <serial_number>XXXXXXXXXXXXX</serial_number>
      </component>
      <component type='HARDWARE'>
         <name>FpgaCompressor</name>
         <part_number>10089996</part_number>
         <serial_number>XXXXXXXXXX</serial_number>
         <revision>X06.15</revision>
         <operational_hours>780</operational_hours>
         <pm_hours_remaining>474</pm_hours_remaining>
      </component>
   </components>
</body>
"
                );
        }
    }
}