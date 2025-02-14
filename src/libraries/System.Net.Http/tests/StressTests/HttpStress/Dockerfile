ARG SDK_BASE_IMAGE=mcr.microsoft.com/dotnet/nightly/sdk:6.0-bullseye-slim
FROM $SDK_BASE_IMAGE

WORKDIR /app
COPY . .

# Pulling the msquic Debian package from msquic-ci public pipeline and from a hardcoded build.
# Note that this is a temporary solution until we have properly published Linux packages.
# Also note that in order to update to a newer msquic build, you have update this link.
ARG MSQUIC_PACKAGE=libmsquic_1.9.0_amd64.deb
ARG PACKAGES_DIR=UnsignedUnixPackages
RUN wget 'https://dev.azure.com/dnceng/9ee6d478-d288-47f7-aacc-f6e6d082ae6d/_apis/build/builds/1426627/artifacts?artifactName=UnsignedUnixPackages&api-version=6.0&%24format=zip' -O "$PACKAGES_DIR".zip
RUN apt-get update
RUN apt-get install unzip
RUN unzip $PACKAGES_DIR.zip
RUN dpkg -i $PACKAGES_DIR/$MSQUIC_PACKAGE
RUN rm -rf $PACKAGES_DIR*

ARG VERSION=7.0
ARG CONFIGURATION=Release

RUN dotnet build -c $CONFIGURATION \
    -p:TargetingPacksTargetsLocation=/live-runtime-artifacts/targetingpacks.targets \
    -p:MicrosoftNetCoreAppRefPackDir=/live-runtime-artifacts/microsoft.netcore.app.ref/ \
    -p:MicrosoftNetCoreAppRuntimePackDir=/live-runtime-artifacts/microsoft.netcore.app.runtime.linux-x64/$CONFIGURATION/

# Enable dump collection
ENV COMPlus_DbgEnableMiniDump=1
ENV COMPlus_DbgMiniDumpType=MiniDumpWithFullMemory
ENV COMPlus_DbgMiniDumpName="/share/coredump.%p"

EXPOSE 5001

ENV VERSION=$VERSION
ENV CONFIGURATION=$CONFIGURATION
ENV HTTPSTRESS_ARGS=''
CMD /live-runtime-artifacts/testhost/net$VERSION-Linux-$CONFIGURATION-x64/dotnet exec \
    ./bin/$CONFIGURATION/net$VERSION/HttpStress.dll $HTTPSTRESS_ARGS
