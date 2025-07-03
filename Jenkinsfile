pipeline {
    agent any
    
    triggers {
        githubPush()
    }
    
    stages {
        stage('Checkout') {
            steps {
                echo 'Fazendo checkout do código...'
                checkout scm
            }
        }
        
        stage('Restore & Build') {
            steps {
                echo 'Restaurando dependências e compilando...'
                sh 'dotnet restore'
                sh 'dotnet build --configuration Release'
            }
        }
        
        stage('Test') {
            steps {
                echo '🧪 Executando testes...'
                sh 'dotnet test Nfe.Tests --configuration Release --verbosity normal'
            }
        }
        
        stage('Publish') {
            steps {
                echo '📋 Publicando aplicação...'
                sh 'dotnet publish Nfe.Api --configuration Release --output ./publish'
                archiveArtifacts artifacts: 'publish/**/*', fingerprint: true
            }
        }
    }
    
    post {
        success {
            echo 'Build realizado com sucesso'
        }
        failure {
            echo 'Build falhou'
        }
        always {
            echo 'Pipeline finalizado.'
        }
    }
}