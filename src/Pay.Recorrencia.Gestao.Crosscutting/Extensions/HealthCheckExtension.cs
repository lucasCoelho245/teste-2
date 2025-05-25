using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Pay.Recorrencia.Gestao.Crosscutting.Health;

namespace Pay.Recorrencia.Gestao.Crosscutting.Extensions
{
    public static class HealthCheckExtension
    {
        public static IServiceCollection AddHealthChecksInjection(this IServiceCollection services, string connectionString, string? kafkaBootstrapServers = null)
        {
            services.AddHealthChecks()
                .AddCheck<ExampleHealthCheck>("Example_Healthy")
                //.AddSqlServer(
                //    connectionString: connectionString,
                //    name: "Banco de dados",
                //    timeout: TimeSpan.FromSeconds(10)
                //)
                ;

            //if (!string.IsNullOrEmpty(kafkaBootstrapServers))
            //{
            //    var kafkaConfig = new ProducerConfig { BootstrapServers = kafkaBootstrapServers };
            //    services.AddHealthChecks().AddKafka(
            //        kafkaConfig,
            //        name: "Kafka",
            //        timeout: TimeSpan.FromSeconds(10)
            //    );
            //}

            return services;
        }
    }
}
