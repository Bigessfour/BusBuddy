# Dockerfile for the React Dashboard
FROM node:18

WORKDIR /app

# Copy package files and install dependencies
COPY package*.json ./
RUN npm install

# Copy source code
COPY . .

# Set environment variables
ENV NODE_ENV=development
ENV PORT=5000

# Start the development server with hot reload
CMD ["npm", "start"]

# Expose ports for React app and API
EXPOSE 3000
EXPOSE 5000
