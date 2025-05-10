// Express API for BusBuddy Dashboard
const express = require('express');
const sql = require('mssql');
const cors = require('cors');
require('dotenv').config();

const app = express();
const port = process.env.PORT || 5000;

app.use(cors());
app.use(express.json());

const dbConfig = {
  user: process.env.DB_USER || 'BusBuddyApp',
  password: process.env.DB_PASS || 'App@P@ss!2025',
  server: process.env.DB_HOST || 'localhost',
  database: process.env.DB_NAME || 'BusBuddy',
  options: {
    encrypt: false, // For local dev
    trustServerCertificate: true,
  },
  port: parseInt(process.env.DB_PORT, 10) || 1433,
};

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
