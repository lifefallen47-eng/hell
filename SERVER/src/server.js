const express = require('express');
const http = require('http');
const socketIO = require('socket.io');
const cors = require('cors');
const helmet = require('helmet');
const rateLimit = require('express-rate-limit');
require('dotenv').config();

const app = express();
const server = http.createServer(app);
const io = socketIO(server, {
    cors: {
        origin: process.env.CLIENT_URL || '*',
        methods: ['GET', 'POST']
    }
});

// Middleware
app.use(helmet());
app.use(cors());
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

// Rate limiting
const limiter = rateLimit({
    windowMs: 15 * 60 * 1000, // 15 minutes
    max: 100 // limit each IP to 100 requests per windowMs
});
app.use(limiter);

// Import routes
const authRoutes = require('./routes/auth');
const playerRoutes = require('./routes/players');
const worldRoutes = require('./routes/world');

// Routes
app.use('/api/auth', authRoutes);
app.use('/api/players', playerRoutes);
app.use('/api/world', worldRoutes);

// Health check
app.get('/health', (req, res) => {
    res.json({ status: 'Server is running' });
});

// Socket.IO events
io.on('connection', (socket) => {
    console.log(`[SOCKET] Player connected: ${socket.id}`);

    // Player joined
    socket.on('player-join', (data) => {
        console.log(`[SOCKET] ${data.playerName} joined`);
        io.emit('player-joined', data);
    });

    // Chat message
    socket.on('send-message', (data) => {
        console.log(`[CHAT] ${data.playerName}: ${data.message}`);
        io.emit('receive-message', data);
    });

    // Combat update
    socket.on('combat-update', (data) => {
        socket.broadcast.emit('combat-update', data);
    });

    // World update
    socket.on('world-update', (data) => {
        socket.broadcast.emit('world-update', data);
    });

    // Player disconnected
    socket.on('disconnect', () => {
        console.log(`[SOCKET] Player disconnected: ${socket.id}`);
        io.emit('player-left', { socketId: socket.id });
    });
});

// Error handling
app.use((err, req, res, next) => {
    console.error('[ERROR]', err);
    res.status(500).json({ error: 'Internal server error' });
});

// Start server
const PORT = process.env.PORT || 3000;
server.listen(PORT, () => {
    console.log(`
╔══════════════════════════════════════════╗
║   HELLFALL: ETERNAL ASCENSION - SERVER   ║
║          Server running on :${PORT}           ║
╚══════════════════════════════════════════╝
`);
});

module.exports = { app, server, io };
