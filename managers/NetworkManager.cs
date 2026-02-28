using Godot;

public partial class NetworkManager : Node
{
    private const int DefaultPort = 7000;
    
    private const byte MaxClients = 2;
    
    private SceneTreeTimer _connectionTimer;
    
    [Signal]
    public delegate void ConnectedToServerEventHandler();
    
    [Signal]
    public delegate void ConnectionFailedEventHandler();
    
    [Signal]
    public delegate void PlayerConnectedEventHandler();
    
    [Signal]
    public delegate void PlayerDisconnectedEventHandler();
    
    [Signal]
    public delegate void ServerDisconnectedEventHandler();

    public static NetworkManager Instance { get; private set; }

    public override void _Ready()
    {
        Multiplayer.PeerConnected += OnPeerConnected;
        Multiplayer.PeerDisconnected += OnPeerDisconnected;
        Multiplayer.ServerDisconnected += OnServerDisconnected;
        Multiplayer.ConnectionFailed += OnConnectionFailed;

        Instance = this;
    }

    public void CreateServer()
    {
        ENetMultiplayerPeer peer = new ENetMultiplayerPeer();
        
        Error error = peer.CreateServer(DefaultPort, MaxClients);
        if (error == Error.Ok)
        {
            Multiplayer.MultiplayerPeer = peer;
            GD.PushWarning("Server started on port " + DefaultPort);
            EmitSignalConnectedToServer();
        }
        else
        {
            GD.PushError("Failed to host : " + error);
        }
    }

    public void CreateClient(string hostIp = "127.0.0.1")
    {
        ENetMultiplayerPeer peer = new ENetMultiplayerPeer();
        
        Error error = peer.CreateClient(hostIp, DefaultPort);
        GD.PushError(error);
        if (error == Error.Ok)
        {
            Multiplayer.MultiplayerPeer = peer;
            GD.PushWarning("Connecting to server...");

            _connectionTimer = GetTree().CreateTimer(10.0f);
            _connectionTimer.Timeout += OnTimeout;
        }
        else
        {
            EmitSignalConnectionFailed();
            GD.PushWarning("Failed to join : " + error);
        }
    }
    
    public void DisconnectPlayer()
    {
        var peer = Multiplayer.MultiplayerPeer;
        
        if (peer != null)
        {
            if (peer is ENetMultiplayerPeer)
            {
                peer.Close();
            }
        
            Multiplayer.MultiplayerPeer = null;
            
            GD.PushWarning("Successfully disconnected.");
        }
    }
    
    private void OnTimeout()
    {
        if (Multiplayer.MultiplayerPeer != null && 
            Multiplayer.MultiplayerPeer.GetConnectionStatus() == MultiplayerPeer.ConnectionStatus.Connecting)
        {
            GD.PushError("Connection timed out.");
        
            Multiplayer.MultiplayerPeer.Close();
            Multiplayer.MultiplayerPeer = null;
        
            EmitSignalConnectionFailed();
        }
    }

    private void OnPeerConnected(long id)
    {
        GD.PushWarning($"Player {id} connected!");
        EmitSignalPlayerConnected();
    }
    
    private void OnPeerDisconnected(long id)
    {
        GD.PushWarning($"Player {id} disconnected.");
        EmitSignalPlayerDisconnected();
    }
    
    private void OnServerDisconnected()
    {
        GD.PushWarning("Server disconnected.");
        Multiplayer.MultiplayerPeer = null;
        EmitSignalServerDisconnected();
    }
    
    private void OnConnectionFailed()
    {
        GD.PushError("Connection failed.");
        Multiplayer.MultiplayerPeer = null;
        EmitSignalConnectionFailed();
    }
}
