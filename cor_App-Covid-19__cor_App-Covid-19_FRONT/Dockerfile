# Builds a Docker to deliver dist/

FROM nginx:1.17.10
ARG PATHVERSION
COPY default.nginx.conf /etc/nginx/conf.d/default.conf
COPY ${PATHVERSION} /usr/share/nginx/html
