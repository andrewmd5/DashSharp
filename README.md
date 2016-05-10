# DashSharp
A C# Library to handle your amazon dash probing to execute custom actions. 

### How does it work?

When an Amazon Dash Button wakes up after being pushed, it introduces itself to the network via a small probe. By simply capturing this probe and raising some events, we can turn our dash button into our own internet of things device.

### How to set it up.

Follow the setup steps provided by Amazon, once you reach step 3 (selecting an item you want the dash to purchase), exit the app and uninstall it (it will bomb you with notifications otherwise) 

Once the Dash Button has been connected to your wifi, any computer on your network can see it when it wakes up. By using this library you can write an application to handle it. 


### Hello World

Using the library is easy

```csharp
private static void Main(string[] args)
        {
            Console.Title = "DashSharp - Amazon Dash Button";
            var network = new DashNetwork();
            network.ListenerStarted += network_ListenerStarted;
            network.DashButtonProbed += network_DashProbed;
            try
            {
                network.StartListening();
            }
            catch (PcapMissingException)
            {
                Console.WriteLine("Pcap is missing, please install it.");
            }
            Console.Read();
       }
```

### Multiple physical addresses 

Dash Buttons have two physical addresses, one for when they are waking up/pressed and the other for their sync attempt/shutting down. This library monitors for them, you have to handle the logic. 

### Requirements

You'll neeed [WinPcap](https://www.winpcap.org/) on Windows and Pcap on Linux or OSX. 

### Running code on your dash

An interesting write up can be found [here](http://www.networkworld.com/article/2994131/internet-of-things/inside-the-amazon-dash-button-hack-and-how-to-make-it-useful.html)
