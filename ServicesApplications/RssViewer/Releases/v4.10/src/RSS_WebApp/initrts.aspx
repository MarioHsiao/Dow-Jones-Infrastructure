<%@ Page Language="vb" AutoEventWireup="false" %>
 <%Response.Buffer = True

        Response.Write("<H1>FCS sessionLogin Synchronous Test</H1>  <H3>Dictionary Objects [IN] / XML [OUT]</H3><P>")

        On Error Resume Next

        Dim ContentManager
        Dim ControlDataDict
        Dim TranDataDict
        Dim ReqDataDict
        Dim RespDataStream
        Dim TranID
        Dim DictKey

        ControlDataDict = CreateObject("Scripting.Dictionary", "")
        ControlDataDict.Add("FCS_CD_SESSION_ID", "ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ")

        TranDataDict = CreateObject("Scripting.Dictionary", "")
        TranDataDict.Add("TranName1", "TranValue1")
        TranDataDict.Add("TranName2", "TranValue2")

        ReqDataDict = CreateObject("Scripting.Dictionary", "")


        ReqDataDict.Add("USER_ID", "assd")
        ReqDataDict.Add("USER_PASSWORD", "xdc")
        ReqDataDict.Add("PRODUCT_ID", "12")
        ReqDataDict.Add("USER_FLAVOR", "F")

        ContentManager = CreateObject("Factiva.ContentManager.1", "")

        ContentManager.Query("MSRVS_AUTH_LOGIN", 1, ControlDataDict, 1, TranDataDict, 1, ReqDataDict, 4, RespDataStream, TranID, 20000)
        If (Err.Number <> 0) Then
            Response.Write("<B>Err Number = " & Err.Number & "</B><BR>")
            Response.Write("<B>Err Source = " & Err.Source & "</B><BR>")
            Response.Write("<B>Err Description = " & Err.Description & "</B><BR>")
        Else
            Response.Write("<HR><BR><B>ControlDataDict After Transaction</B><BR>" & vbCrLf)
            Response.Write("<Table border=1>" & vbCrLf)
            For Each DictKey In ControlDataDict
                Response.Write("<TR><TD><PRE>" & DictKey & "</PRE></TD><TD><PRE>" & ControlDataDict(DictKey) & "</PRE></TD></TR>" & vbCrLf)
            Next
            Response.Write("</Table><P>" & vbCrLf)

            Response.Write("<BR><B>Response Data After Transaction</B><BR>" & vbCrLf)
            'Response.Write WriteXMLString(RespDataStream)
            Response.Write(RespDataStream)
        End If


        'Response.Cookies("SessionID") = ControlDataDict("FCS_CD_SESSION_ID")


        ContentManager = Nothing
        ControlDataDict = Nothing
        TranDataDict = Nothing
        ReqDataDict = Nothing
        RespDataStream = Nothing
        %>
