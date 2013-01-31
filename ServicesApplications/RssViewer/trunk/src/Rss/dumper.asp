<%@ Language=VBScript %>
<% Option Explicit

Dim UserId, Password, ProductId
Dim SessionId

UserId = cstr(Request("userid"))
Password = cstr(Request("password"))
ProductId = cstr(Request("productid"))
   
on error resume next

Dim contentManager
err.clear
Set contentManager = Server.CreateObject("Factiva.ContentManager.1")
response.write err.number & "<br>"
response.write err.description & "<br>"
response.write err.source & "<br>"

' Run a login transaction and dump the result

Dim controlDataDict
Dim transDataDict
Dim reqDataDict
Dim responseFormat, defaultTimeOut
Dim functionType, transID
Dim respDataStream

err.clear
Set controlDataDict = Server.CreateObject("Scripting.Dictionary")
response.write err.number
Set transDataDict = Server.CreateObject("Scripting.Dictionary")
Set reqDataDict = Server.CreateObject("Scripting.Dictionary")

responseFormat = 6
defaultTimeOut = 61000
functionType = "MSRVS_AUTH_LOGIN"

controlDataDict.Add "FCS_CD_SESSION_ID", ""
controlDataDict.Add "FCS_CD_CHUNKING", False

transDataDict.Add "TranName1", "TranValue1"
transDataDict.Add "TranName2", "TranValue2"

reqDataDict.Add	"USER_ID", UserId
reqDataDict.Add	"USER_PASSWORD", Password
reqDataDict.Add	"PRODUCT_ID", ProductId
reqDataDict.Add	"USER_FLAVOR", "F"

On Error Resume next
contentManager.PostRequest functionType, 1, controlDataDict, 1, transDataDict, 1, reqDataDict, transID

If (Err.number <> 0) Then
	Response.Write("<BR>ContentManager.PostRequest Error")
	Response.Write("<BR>Err Number = " & Err.number)
	Response.Write("<BR>Err Source = " & Err.source)
	Response.Write("<BR>Err Description = " & Err.description)
	Response.Write("<BR><BR>")
Else
	contentManager.GetResponse functionType, 1, controlDataDict, 1, transDataDict, responseFormat, respDataStream, transID, defaultTimeOut
	If (Err.number <> 0) Then
		Response.Write("<BR>ContentManager.GetResponse Error")
		Response.Write("<BR>Err Number = " & Err.number)
		Response.Write("<BR>Err Source = " & Err.source)
		Response.Write("<BR>Err Description = " & Err.description)
		Response.Write("<BR><BR>")
	Else
		response.Write("<PRE>")
		response.Write(server.HTMLEncode(respDataStream.xml))
 		response.Write("</PRE><P>")
	End If
End If

SessionId = ControlDataDict("FCS_CD_SESSION_ID")
Response.Write("<BR>SessionID = ")
Response.Write SessionId


' Run a simple search against Index Services and dump the result

         
Dim SearchControlDataDict    
Dim SearchTranDataDict      
Dim SearchReqDataDict      
Dim SearchRespDataStream      
Dim SearchTranID                  
Dim SearchFunctionType      
 
Set SearchControlDataDict = Server.CreateObject("Scripting.Dictionary") 
Set SearchTranDataDict    = Server.CreateObject("Scripting.Dictionary") 
Set SearchReqDataDict     = Server.CreateObject("Scripting.Dictionary") 
 
SearchControlDataDict.Add "FCS_CD_SESSION_ID", SessionId
SearchControlDataDict.Add "FCS_CD_CHUNKING",   True 
     
SearchTranDataDict.Add "TranName1", "TranValue1" 
SearchTranDataDict.Add "TranName2", "TranValue2" 
 
SearchFunctionType = "INDEX_Search"

SearchReqDataDict.Add  "bss",  "bush"
SearchReqDataDict.Add  "rrs",  ""
SearchReqDataDict.Add  "nhr",  "010"
SearchReqDataDict.Add  "hpc",  "10" 
SearchReqDataDict.Add  "hrd",  "" 
SearchReqDataDict.Add  "crd",  "" 
SearchReqDataDict.Add  "so",   "y" 
SearchReqDataDict.Add  "is",   "y" 
SearchReqDataDict.Add  "qdf",  "mdy" 
SearchReqDataDict.Add  "qcs",  "dj"
SearchReqDataDict.Add  "ics",  "y"


Response.Write("<BR>Calling ContentManager.PostRequest...")
Response.Flush
Err.Clear

	Response.Write "Getting Response at time: " & now & "<BR>"
ContentManager.PostRequest   SearchFunctionType,       _ 
                             1, SearchControlDataDict,   _ 
                             1, SearchTranDataDict,      _ 
                             1, SearchReqDataDict,       _ 
                             SearchTranID 
 
	Response.Write "Got Response at time: " & now & "<BR>"
If (Err.number <> 0) Then 

    Response.Write("<BR>ContentManager.PostRequest Error")        
    Response.Write("<BR>Err Number = " & Err.number) 
    Response.Write("<BR>Err Source = " & Err.source) 
    Response.Write("<BR>Err Description = " & Err.description) 
    Response.Write("<BR><BR>") 

Else 

    Response.Write("<BR>ContentManager.PostRequest Successful TranId = " & SearchTranID)

    Dim getFlag 
    Dim moreData 
     
    getFlag = 1 
                 
    Do  ' loop until getFlag = 0 
       
        Response.Write("<BR>Calling ContentManager.GetResponse...")
        Response.Flush
        
        ContentManager.GetResponse   SearchFunctionType,        _ 
                                     1, SearchControlDataDict,  _ 
                                     1, SearchTranDataDict,     _ 
                                     6, SearchRespDataStream,   _ 
                                     SearchTranID, 30000       
         
        If (Err.number <> 0) Then 
 
            Response.Write("<BR>ContentManager.GetResponse Error.")   
            Response.Write("<BR>Err Number = " & Err.number) 
            Response.Write("<BR>Err Source = " & Err.source) 
            Response.Write("<BR>Err Description = " & Err.description) 
            Response.Write("<BR><BR>")     
            
            getFlag = 0    
 
        Else 
        
            Response.Write("<BR>ContentManager.GetResponse Successful")
        
            Response.Write(server.HTMLEncode(SearchRespDataStream.xml))

            formatResponse(SearchRespDataStream)
         
            moreData = SearchControlDataDict.Item("FCS_CD_MORE_DATA")
            
            Response.Write "<BR>moreData = " & moreData & "<BR>" 
 
            If (moreData = False) Then 
 
                getFlag = 0 

            End If 
 
        End If 
	
    Loop Until getFlag = 0 
 
End If 

 
Set SearchControlDataDict = Nothing 
Set SearchTranDataDict = Nothing 
Set SearchReqDataDict = Nothing 
Set SearchRespDataStream = Nothing 
Set SearchTranID = Nothing 


Sub formatResponse(nodes)

    Dim nodeList
    Dim xNode

    Response.Write "<BR><BR>"
    Set nodeList = Nodes.selectNodes("IndexSearchResponse/ResultSet/Result/ReplyItem/Title/Headline")
    For each xNode in nodeList
        Response.Write "text = " & left(xNode.Text,137)
    Next
End Sub


Set ContentManager = Nothing
Set ControlDataDict = Nothing
Set TranDataDict = Nothing
Set ReqDataDict = Nothing
Set RespDataStream = Nothing
%>