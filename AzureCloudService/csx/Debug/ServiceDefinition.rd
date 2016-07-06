<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="AzureCloudService" generation="1" functional="0" release="0" Id="a9b04cb3-199f-48b8-ab8d-df4896ef792a" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="AzureCloudServiceGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="WcfWebService:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/AzureCloudService/AzureCloudServiceGroup/LB:WcfWebService:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="WcfWebService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/AzureCloudService/AzureCloudServiceGroup/MapWcfWebService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="WcfWebServiceInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/AzureCloudService/AzureCloudServiceGroup/MapWcfWebServiceInstances" />
          </maps>
        </aCS>
        <aCS name="WorkerRole1:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/AzureCloudService/AzureCloudServiceGroup/MapWorkerRole1:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="WorkerRole1Instances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/AzureCloudService/AzureCloudServiceGroup/MapWorkerRole1Instances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:WcfWebService:Endpoint1">
          <toPorts>
            <inPortMoniker name="/AzureCloudService/AzureCloudServiceGroup/WcfWebService/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapWcfWebService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureCloudService/AzureCloudServiceGroup/WcfWebService/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapWcfWebServiceInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/AzureCloudService/AzureCloudServiceGroup/WcfWebServiceInstances" />
          </setting>
        </map>
        <map name="MapWorkerRole1:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureCloudService/AzureCloudServiceGroup/WorkerRole1/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapWorkerRole1Instances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/AzureCloudService/AzureCloudServiceGroup/WorkerRole1Instances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="WcfWebService" generation="1" functional="0" release="0" software="C:\Users\SUPINFO.NET\documents\visual studio 2013\Projects\AzureCloudService\AzureCloudService\csx\Debug\roles\WcfWebService" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="8080" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;WcfWebService&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;WcfWebService&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;WorkerRole1&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="LocalStorage1" defaultAmount="[1024,1024,1024]" defaultSticky="false" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/AzureCloudService/AzureCloudServiceGroup/WcfWebServiceInstances" />
            <sCSPolicyUpdateDomainMoniker name="/AzureCloudService/AzureCloudServiceGroup/WcfWebServiceUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/AzureCloudService/AzureCloudServiceGroup/WcfWebServiceFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="WorkerRole1" generation="1" functional="0" release="0" software="C:\Users\SUPINFO.NET\documents\visual studio 2013\Projects\AzureCloudService\AzureCloudService\csx\Debug\roles\WorkerRole1" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="-1" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;WorkerRole1&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;WcfWebService&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;WorkerRole1&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="Backup" defaultAmount="[1024,1024,1024]" defaultSticky="false" kind="Directory" />
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/AzureCloudService/AzureCloudServiceGroup/WorkerRole1Instances" />
            <sCSPolicyUpdateDomainMoniker name="/AzureCloudService/AzureCloudServiceGroup/WorkerRole1UpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/AzureCloudService/AzureCloudServiceGroup/WorkerRole1FaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="WcfWebServiceUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="WorkerRole1UpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="WcfWebServiceFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="WorkerRole1FaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="WcfWebServiceInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="WorkerRole1Instances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="347b51ed-3117-4198-9566-fc9dad582c45" ref="Microsoft.RedDog.Contract\ServiceContract\AzureCloudServiceContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="8b191a29-5569-4e8d-85a1-5d9c9f86b060" ref="Microsoft.RedDog.Contract\Interface\WcfWebService:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/AzureCloudService/AzureCloudServiceGroup/WcfWebService:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>