<%
Response.Buffer = True

Response.Write "<H1>FCS sessionLogin Synchronous Test</H1>  <H3>Dictionary Objects [IN] / XML [OUT]</H3><P>"

On Error Resume Next

Dim ContentManager
Dim ControlDataDict
Dim TranDataDict
Dim ReqDataDict
Dim RespDataStream
Dim TranID
Dim DictKey

Set ControlDataDict = CreateObject("Scripting.Dictionary")
ControlDataDict.Add "FCS_CD_SESSION_ID", "ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ"

Set TranDataDict = CreateObject("Scripting.Dictionary")
TranDataDict.Add "TranName1", "TranValue1"
TranDataDict.Add "TranName2", "TranValue2"

Set ReqDataDict = CreateObject("Scripting.Dictionary")


ReqDataDict.Add "USER_ID", "assd"
ReqDataDict.Add "USER_PASSWORD", "xdc"
ReqDataDict.Add "PRODUCT_ID", "12"
ReqDataDict.Add "USER_FLAVOR", "F"

Set ContentManager = Server.CreateObject("Factiva.ContentManager.1")

ContentManager.Query "MSRVS_AUTH_LOGIN", 1, ControlDataDict, 1, TranDataDict, 1, ReqDataDict, 4, RespDataStream, TranID, 20000 
if (Err.number <> 0) then
	Response.Write "<B>Err Number = " & Err.number & "</B><BR>"
	Response.Write "<B>Err Source = " & Err.source & "</B><BR>"
	Response.Write "<B>Err Description = " & Err.description & "</B><BR>"
else
	Response.Write "<HR><BR><B>ControlDataDict After Transaction</B><BR>" & vbCRLF 
	Response.Write "<Table border=1>" & vbCRLF 
	For Each DictKey In ControlDataDict
		Response.Write "<TR><TD><PRE>" & DictKey & "</PRE></TD><TD><PRE>" & ControlDataDict(DictKey) & "</PRE></TD></TR>" & vbCRLF
	Next
	Response.Write "</Table><P>" & vbCRLF

	Response.Write "<BR><B>Response Data After Transaction</B><BR>" & vbCRLF 
	'Response.Write WriteXMLString(RespDataStream)
	Response.Write RespDataStream
end if


Response.Cookies("SessionID") = ControlDataDict("FCS_CD_SESSION_ID")


Set ContentManager = Nothing
Set ControlDataDict = Nothing
Set TranDataDict = Nothing
Set ReqDataDict = Nothing
Set RespDataStream = Nothing
%>