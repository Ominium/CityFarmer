using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using MySql.Data.MySqlClient;
public class Maria 
{
    public static MySqlConnection SqlConnection;

    static readonly string _ipAddress = "13.124.59.40";
    static readonly string _dbId = "root";
    static readonly string _dbPw = "dnflskfk1";
    static readonly string _dbName = "unityDB";

    public static string strConnection = string.Format("server={0};uid={1};pwd={2};database={3};charset=utf8 ;", _ipAddress, _dbId, _dbPw, _dbName);
  
    //데이터 , 삽입, 업데이트 쿼리 함수
    public static bool OnInsertOrUpdateRequest(string str_query)
    {
        try
        {
            MySqlCommand sqlCommand = new MySqlCommand();
            sqlCommand.Connection = SqlConnection;
            sqlCommand.CommandText = str_query;

            SqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            SqlConnection.Close();
            return true;
        }
        catch(System.Exception e)
        {
            Debug.Log(e.ToString());
            return false;
        }
    }
    // 데이터 조회시 사용
    public static DataSet OnSelectRequest(string p_query,string table_name)
    {
        try
        {
            SqlConnection.Open();
            MySqlCommand sqlCommand = new MySqlCommand();
            sqlCommand.Connection = SqlConnection;
            sqlCommand.CommandText = p_query;
            MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(sqlCommand);
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet, table_name);
            SqlConnection.Close();

            return dataSet;
        }catch(System.Exception e)
        {
            Debug.Log(e.ToString());

            return null;
        }
    }
   
}
