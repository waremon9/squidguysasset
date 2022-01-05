using System;
using System.Linq;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleSheetClient : MySingleton<GoogleSheetClient>
{
    [SerializeField] private string spreadsheetId = "1T_mQgxKeAoG7QKglKpxbt0lWdOyAqubVz8vBZlgBb_8";
    [SerializeField] private string[] scopes = { SheetsService.Scope.Spreadsheets };
    [SerializeField] private string _credentialsPath = "Assets/GoogleSheet";
    [SerializeField] private string _credentialsFile = "Credentials.json";
    private SheetsService _service = null;
    public bool Connected;

    public override bool DoDestroyOnLoad { get; }

    [ContextMenu("Connect")]

    private void Start()
    {
        Connect();
        ReadAsync("A2:A3", 2000);
    }
    public async void Connect()
    {
        Debug.Log("Connecting...");
        await ConnectAsync();
        Debug.Log("Connected!");
    }

    public async Task ConnectAsync()
    {
        string credentialsDirectoryPath = Path.Combine(Application.dataPath, _credentialsPath);
        string credentialsPath = Path.Combine(credentialsDirectoryPath, _credentialsFile);
        if (!Directory.Exists(credentialsDirectoryPath))
        {
            Directory.CreateDirectory(credentialsDirectoryPath);
        }
        UserCredential credential;
        GoogleClientSecrets secrets;

        Debug.Log("Reading Credentials...");
        using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
        {
            secrets = await GoogleClientSecrets.FromStreamAsync(stream);
        }

        string tokenPath = Path.Combine(Application.persistentDataPath, _credentialsPath);

        Debug.Log("Opening connection...");
        credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(secrets.Secrets, scopes, "user", CancellationToken.None, new FileDataStore(tokenPath, true));

        Debug.Log($"Credential token saved to: {tokenPath}");

        _service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential
        });

        Connected = true;
    }

    public async Task<string?> GetCellData(string range, int timeout)
    {
        if (!await AwaitForConnection(timeout))
        {
            return null;
        }
        var request = _service.Spreadsheets.Values.Get(spreadsheetId, range);

        ValueRange response = await request.ExecuteAsync();

        IList<IList<object>> values = response.Values;
        if (values !=null && values.Count > 0 && values[0].Count> 0)
        {
            return values[0][0].ToString();
        }
        else
        {
            Debug.LogError("no data found.");
            return null;
        }

    }

    public async Task<bool> WriteLineData( string range, int timeout, params object[] data)
    {
        if (!await AwaitForConnection(timeout))
        {
            return false;
        }

        ValueRange valueRange = new ValueRange();
        IList<IList<object>> values = new List<IList<object>> { new List<object>() };
        values[0] = data.ToList();
        valueRange.Values = values;

        var valueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
        var request = _service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
        request.ValueInputOption = valueInputOption;

        UpdateValuesResponse response = await request.ExecuteAsync();
        return true;
    }

    private async Task<bool> AwaitForConnection(int timeout)
    {
        CancellationTokenSource source = new CancellationTokenSource(timeout);

        while (!Connected)
        {
            await Task.Delay(16);
            if (source.Token.IsCancellationRequested)
            {
                Debug.LogError("Connection timed out");
                return false;
            }
        }
        return true;
    }

    public async void ReadAsync(string range, int timeout)
    {
        string? value =await GetCellData(range, timeout);
        Debug.Log($"test: {range} = {value}");
    }
}
