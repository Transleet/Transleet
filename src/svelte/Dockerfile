FROM node:17-alpine AS build
RUN yarn global add pnpm
COPY package.json .
RUN pnpm install
COPY . .
RUN pnpm build

FROM node:17-alpine
WORKDIR /app
COPY --from=build /build .
COPY --from=build /package.json .
EXPOSE 3000
ENTRYPOINT [ "node","." ]