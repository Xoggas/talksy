# ---------- СТАДИЯ 1: сборка Angular ----------
FROM node:20-alpine AS build
WORKDIR /app

# Копируем package.json и package-lock.json (или yarn.lock)
COPY package*.json ./

# Устанавливаем зависимости
RUN npm ci

# Копируем остальной код
COPY . .

# Собираем проект (папка /app/dist/<project-name>)
RUN npm run build -- --configuration production


# ---------- СТАДИЯ 2: деплой через nginx ----------
FROM nginx:alpine

# Копируем собранный фронт из предыдущего контейнера
COPY --from=build /app/dist/talksy/browser /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
