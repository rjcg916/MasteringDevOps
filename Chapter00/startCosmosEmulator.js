const net = require('net');
const { exec } = require('child_process');

const port = 8081; // Default port for Cosmos DB Emulator

const client = new net.Socket();

// Attempt to connect to the Cosmos DB Emulator port
client.connect(port, 'localhost', function() {
    console.log('Cosmos DB Emulator is already running.');
    client.destroy(); // close the connection
});

client.on('error', function(error) {
    console.log('Cosmos DB Emulator is not running. Attempting to start...');
    // Adjust the command below according to your Cosmos DB Emulator path and requirements
    exec('"C:\\Program Files\\Azure Cosmos DB Emulator\\CosmosDB.Emulator.exe"', (err, stdout, stderr) => {
        if (err) {
            console.error(`Error starting Cosmos DB Emulator: ${err}`);
            return;
        }
        console.log('Cosmos DB Emulator started successfully');
    });
});