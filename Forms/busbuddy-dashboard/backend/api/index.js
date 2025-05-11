// Express API for BusBuddy Dashboard
const express = require('express');
const sql = require('mssql');
const cors = require('cors');
require('dotenv').config();

const app = express();
const port = process.env.PORT || 5000;

// Enable CORS with specific configuration for Docker environment
app.use(cors({
  origin: ['http://localhost:3000', 'http://dashboard:3000', 'http://webapp:5000', 'http://webapp:5001'],
  methods: ['GET', 'POST', 'PUT', 'DELETE'],
  allowedHeaders: ['Content-Type', 'Authorization'],
  credentials: true
}));
app.use(express.json());

const dbConfig = {
  user: process.env.DB_USER || 'BusBuddyApp',
  password: process.env.DB_PASSWORD || 'App@P@ss!2025', // Updated to match Docker env var name
  server: process.env.DB_SERVER || 'localhost',  // Updated to match Docker env var name
  database: process.env.DB_DATABASE || 'BusBuddy', // Updated to match Docker env var name
  options: {
    encrypt: false, 
    trustServerCertificate: true,
  },
  port: parseInt(process.env.DB_PORT, 10) || 1433,
};

// Log database connection details (without password)
console.log(`Connecting to SQL Server at ${dbConfig.server}:${dbConfig.port} as ${dbConfig.user}, database: ${dbConfig.database}`);

app.get('/api/busroutes', async (req, res) => {
  try {
    await sql.connect(dbConfig);
    const result = await sql.query('SELECT * FROM BusRoutes');
    res.json(result.recordset);
  } catch (err) {
    res.status(500).json({ message: err.message || 'Database error' });
  }
});

app.listen(port, () => {
  console.log(`API server running on port ${port}`);
});
