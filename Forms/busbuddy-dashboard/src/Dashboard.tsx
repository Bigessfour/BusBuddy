import React, { useEffect, useState, useMemo } from 'react';
import {
  DataGrid,
  GridColDef,
  GridToolbar,
  GridColumnVisibilityModel,
} from '@mui/x-data-grid';
import {
  AppBar,
  Toolbar,
  Typography,
  IconButton,
  Card,
  CardContent,
  CircularProgress,
  Snackbar,
  Alert,
  Switch,
  FormControlLabel,
  Box,
  Button,
} from '@mui/material';
import RefreshIcon from '@mui/icons-material/Refresh';
import { createTheme, ThemeProvider, CssBaseline } from '@mui/material';
import axios from 'axios';

const COLUMN_PREFERENCE_KEY = 'busbuddy_dashboard_columns';

export interface BusRoute {
  RouteID: number;
  RouteName: string;
  StartLocation: string;
  EndLocation: string;
  Distance: number;
  LastUpdated: string;
}

const columns: GridColDef[] = [
  { field: 'RouteID', headerName: 'Route ID', width: 100 },
  { field: 'RouteName', headerName: 'Route Name', width: 180, flex: 1 },
  { field: 'StartLocation', headerName: 'Start Location', width: 180, flex: 1 },
  { field: 'EndLocation', headerName: 'End Location', width: 180, flex: 1 },
  {
    field: 'Distance',
    headerName: 'Distance (mi)',
    width: 130,
    valueFormatter: (params) => params.value?.toFixed(2),
  },
  {
    field: 'LastUpdated',
    headerName: 'Last Updated',
    width: 180,
    valueFormatter: (params) => {
      const date = new Date(params.value);
      return date.toLocaleString('en-US', {
        month: '2-digit',
        day: '2-digit',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit',
      });
    },
  },
];

const getInitialColumnVisibility = (): GridColumnVisibilityModel => {
  const saved = localStorage.getItem(COLUMN_PREFERENCE_KEY);
  if (saved) return JSON.parse(saved);
  return {};
};

const Dashboard: React.FC = () => {
  const [routes, setRoutes] = useState<BusRoute[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [darkMode, setDarkMode] = useState(false);
  const [columnVisibilityModel, setColumnVisibilityModel] = useState<GridColumnVisibilityModel>(getInitialColumnVisibility());

  const theme = useMemo(
    () =>
      createTheme({
        palette: {
          mode: darkMode ? 'dark' : 'light',
          primary: { main: '#1976d2' },
        },
      }),
    [darkMode]
  );

  const fetchRoutes = async () => {
    setLoading(true);
    setError(null);
    try {
      const res = await axios.get('/api/busroutes');
      setRoutes(res.data);
    } catch (err: any) {
      setError(err?.response?.data?.message || 'Failed to connect to database');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchRoutes();
    // eslint-disable-next-line
  }, []);

  useEffect(() => {
    localStorage.setItem(COLUMN_PREFERENCE_KEY, JSON.stringify(columnVisibilityModel));
  }, [columnVisibilityModel]);

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" sx={{ flexGrow: 1 }}>
            BusBuddy Dashboard
          </Typography>
          <FormControlLabel
            control={<Switch checked={darkMode} onChange={() => setDarkMode((d) => !d)} color="default" />}
            label={darkMode ? 'Dark' : 'Light'}
          />
          <IconButton color="inherit" onClick={fetchRoutes} aria-label="refresh">
            <RefreshIcon />
          </IconButton>
        </Toolbar>
      </AppBar>
      <Box sx={{ p: 2, maxWidth: 1200, margin: 'auto' }}>
        <Card>
          <CardContent>
            <Box sx={{ height: 600, width: '100%' }}>
              {loading ? (
                <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100%' }}>
                  <CircularProgress />
                </Box>
              ) : (
                <DataGrid
                  rows={routes}
                  columns={columns}
                  getRowId={(row) => row.RouteID}
                  pageSizeOptions={[10, 25, 50]}
                  initialState={{ pagination: { paginationModel: { pageSize: 10 } } }}
                  checkboxSelection
                  disableRowSelectionOnClick
                  slots={{ toolbar: GridToolbar }}
                  columnVisibilityModel={columnVisibilityModel}
                  onColumnVisibilityModelChange={setColumnVisibilityModel}
                  sx={{ backgroundColor: 'background.paper' }}
                  autoHeight={false}
                  density="comfortable"
                  />
              )}
            </Box>
          </CardContent>
        </Card>
        <Snackbar open={!!error} autoHideDuration={6000} onClose={() => setError(null)}>
          <Alert severity="error" onClose={() => setError(null)}>
            {error}
          </Alert>
        </Snackbar>
      </Box>
    </ThemeProvider>
  );
};

export default Dashboard;
