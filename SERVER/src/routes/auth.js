const express = require('express');
const router = express.Router();
const jwt = require('jsonwebtoken');
const bcrypt = require('bcryptjs');

const JWT_SECRET = process.env.JWT_SECRET || 'your-secret-key';

// In production, use actual database
const users = new Map(); // Temporary storage

/**
 * Register new player
 */
router.post('/register', async (req, res) => {
    try {
        const { username, password, email } = req.body;

        // Validation
        if (!username || !password || !email) {
            return res.status(400).json({ error: 'Missing required fields' });
        }

        // Check if user exists
        if (users.has(username)) {
            return res.status(400).json({ error: 'Username already taken' });
        }

        // Hash password
        const hashedPassword = await bcrypt.hash(password, 10);

        // Create user
        const user = {
            username,
            email,
            password: hashedPassword,
            createdAt: new Date()
        };

        users.set(username, user);

        console.log(`[AUTH] New user registered: ${username}`);

        res.status(201).json({
            message: 'User registered successfully',
            username
        });
    } catch (error) {
        console.error('[AUTH ERROR]', error);
        res.status(500).json({ error: 'Registration failed' });
    }
});

/**
 * Login player
 */
router.post('/login', async (req, res) => {
    try {
        const { username, password } = req.body;

        // Validation
        if (!username || !password) {
            return res.status(400).json({ error: 'Missing credentials' });
        }

        // Check user exists
        const user = users.get(username);
        if (!user) {
            return res.status(401).json({ error: 'Invalid credentials' });
        }

        // Check password
        const passwordMatch = await bcrypt.compare(password, user.password);
        if (!passwordMatch) {
            return res.status(401).json({ error: 'Invalid credentials' });
        }

        // Generate JWT
        const token = jwt.sign(
            { username, email: user.email },
            JWT_SECRET,
            { expiresIn: '24h' }
        );

        console.log(`[AUTH] User logged in: ${username}`);

        res.json({
            message: 'Login successful',
            token,
            username
        });
    } catch (error) {
        console.error('[AUTH ERROR]', error);
        res.status(500).json({ error: 'Login failed' });
    }
});

/**
 * Verify token
 */
router.post('/verify', (req, res) => {
    try {
        const token = req.headers.authorization?.split(' ')[1];
        if (!token) {
            return res.status(401).json({ error: 'No token provided' });
        }

        const decoded = jwt.verify(token, JWT_SECRET);
        res.json({ valid: true, user: decoded });
    } catch (error) {
        res.status(401).json({ valid: false, error: 'Invalid token' });
    }
});

module.exports = router;
