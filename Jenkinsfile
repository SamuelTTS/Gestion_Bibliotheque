pipeline {
    agent any
    
     tools {
        // Le nom ici doit être EXACTEMENT le même que celui mis dans "Tools"
        dotnetsdk 'dotnet' 
    }

    environment {
        // Variables pour Docker Hub
        DOCKER_USER = "samueltts"
        IMAGE_NAME  = "gestion-bibliotheque"
        REGISTRY    = "docker.io"
    }

    stages {
        stage('1. Checkout') {
            steps {
                // Récupère le code depuis votre repo Git (GitHub/GitLab)
                checkout scm
                echo "Code récupéré avec succès."
            }
        }

        stage('2. Build .NET') {
            steps {
                // Restaure les dépendances NuGet et compile
                sh 'dotnet restore'
                sh 'dotnet build --configuration Release'
            }
        }

        stage('3. Unit Tests') {
            steps {
                // Exécute les tests et génère un rapport XML compatible Jenkins
                sh 'dotnet test --no-build --configuration Release --logger "junit;LogFileName=results.xml"'
            }
            post {
                always {
                    // Affiche les graphiques de test dans l'interface Jenkins
                    junit '**/results.xml'
                }
            }
        }

        stage('4. Dockerize') {
            steps {
                // Construction de l'image avec un tag unique (le numéro du build Jenkins)
                sh "docker build -t ${DOCKER_USER}/${IMAGE_NAME}:${env.BUILD_ID} ."
                sh "docker tag ${DOCKER_USER}/${IMAGE_NAME}:${env.BUILD_ID} ${DOCKER_USER}/${IMAGE_NAME}:latest"
            }
        }

        stage('5. Push Registry') {
            steps {
                // Connexion sécurisée à Docker Hub via les Credentials Jenkins
                withCredentials([usernamePassword(credentialsId: 'docker-hub-login', passwordVariable: 'PASS', usernameVariable: 'USER')]) {
                    sh "echo \$PASS | docker login -u \$USER --password-stdin"
                    sh "docker push ${DOCKER_USER}/${IMAGE_NAME}:${env.BUILD_ID}"
                    sh "docker push ${DOCKER_USER}/${IMAGE_NAME}:latest"
                }
            }
        }

        stage('6. Deploy Staging') {
            steps {
                // Déploiement local sur le serveur de test
                echo "Déploiement sur l'environnement de Staging..."
                sh "docker compose up -d --force-recreate staging-service"
            }
        }
    }

    post {
        success {
            echo "Le pipeline s'est terminé avec succès !"
            // Ici vous pourrez ajouter Slack ou Email plus tard
        }
        failure {
            echo "Le pipeline a échoué. Vérifiez les logs."
        }
    }
}
