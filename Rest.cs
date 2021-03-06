using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Net;
using System.IO;
using Rest;
using System.Text;

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.Read, Name = "fn_rest_get")]
    public static SqlString GET(SqlString uri, SqlString headers)
    {
        SqlString document;

        WebRequest req = WebRequest.Create(Convert.ToString(uri));
        req.Headers = Headers.SetHeader(headers.ToString());
        req.ContentType = "application/json";

        WebResponse resp = req.GetResponse();
        Stream dataStream = resp.GetResponseStream();
        StreamReader rdr = new StreamReader(dataStream);
        document = (SqlString)rdr.ReadToEnd();

        rdr.Close();
        dataStream.Close();
        resp.Close();

        return (document);
    }

    [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.Read, Name = "fn_rest_post")]
    public static SqlString POST(SqlString uri, SqlString postData, SqlString headers)
    {
        SqlString document;
        byte[] postByteArray = Encoding.UTF8.GetBytes(Convert.ToString(postData));

        WebRequest req = WebRequest.Create(Convert.ToString(uri));
        req.Headers = Headers.SetHeader(headers.ToString());
        req.ContentType = "application/json";
        req.Method = "POST";

        Stream dataStream = req.GetRequestStream();
        dataStream.Write(postByteArray, 0, postByteArray.Length);
        dataStream.Close();

        WebResponse resp = req.GetResponse();
        dataStream = resp.GetResponseStream();
        StreamReader rdr = new StreamReader(dataStream);
        document = (SqlString)rdr.ReadToEnd();

        rdr.Close();
        dataStream.Close();
        resp.Close();

        return (document);
    }



}
