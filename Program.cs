
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClinicaVidaMais
{
    class Program
    {
        static List<Paciente> pacientes = new List<Paciente>();
        static List<Medico> medicos = new List<Medico>();
        static List<Consulta> consultas = new List<Consulta>();

        static void Main(string[] args)
        {
            int opcao;

            do
            {
                Console.Clear();

                Console.WriteLine("==================================");
                Console.WriteLine("       SISTEMA CLÍNICA VIDA+");
                Console.WriteLine("==================================");
                Console.WriteLine("1 - Cadastrar paciente");
                Console.WriteLine("2 - Listar pacientes");
                Console.WriteLine("3 - Cadastrar médico");
                Console.WriteLine("4 - Listar médicos");
                Console.WriteLine("5 - Agendar consulta");
                Console.WriteLine("6 - Listar consultas");
                Console.WriteLine("0 - Sair");
                Console.WriteLine("==================================");
                Console.Write("Escolha uma opção: ");

                if (!int.TryParse(Console.ReadLine(), out opcao))
                    opcao = -1;

                switch (opcao)
                {
                    case 1:
                        CadastrarPaciente();
                        break;

                    case 2:
                        ListarPacientes();
                        break;

                    case 3:
                        CadastrarMedico();
                        break;

                    case 4:
                        ListarMedicos();
                        break;

                    case 5:
                        AgendarConsulta();
                        break;

                    case 6:
                        ListarConsultas();
                        break;

                    case 0:
                        Console.WriteLine("Sistema encerrado!");
                        break;

                    default:
                        Console.WriteLine("Opção inválida!");
                        Pausar();
                        break;
                }

            } while (opcao != 0);
        }

        static void CadastrarPaciente()
        {
            Console.Clear();
            Console.WriteLine("=== CADASTRO DE PACIENTE ===");

            Paciente paciente = new Paciente();

            paciente.Id = pacientes.Count + 1;

            Console.Write("Nome: ");
            paciente.Nome = Console.ReadLine() ?? "";

            Console.Write("Idade: ");
            int.TryParse(Console.ReadLine(), out int idade);
            paciente.Idade = idade;

            Console.Write("Telefone: ");
            paciente.Telefone = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(paciente.Nome) ||
                paciente.Idade <= 0 ||
                string.IsNullOrWhiteSpace(paciente.Telefone))
            {
                Console.WriteLine("Preencha os dados corretamente.");
            }
            else
            {
                pacientes.Add(paciente);
                Console.WriteLine("Paciente cadastrado com sucesso!");
            }

            Pausar();
        }

        static void ListarPacientes()
        {
            Console.Clear();
            Console.WriteLine("=== LISTA DE PACIENTES ===");

            if (pacientes.Count == 0)
                Console.WriteLine("Nenhum paciente cadastrado.");

            foreach (Paciente p in pacientes)
            {
                Console.WriteLine(
                    $"ID: {p.Id} | Nome: {p.Nome} | " +
                    $"Idade: {p.Idade} | Telefone: {p.Telefone}"
                );
            }

            Pausar();
        }

        static void CadastrarMedico()
        {
            Console.Clear();
            Console.WriteLine("=== CADASTRO DE MÉDICO ===");

            Medico medico = new Medico();

            medico.Id = medicos.Count + 1;

            Console.Write("Nome do médico: ");
            medico.Nome = Console.ReadLine() ?? "";

            Console.Write("Especialidade: ");
            medico.Especialidade = Console.ReadLine() ?? "";

            Console.Write("CRM: ");
            medico.Crm = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(medico.Nome) ||
                string.IsNullOrWhiteSpace(medico.Especialidade) ||
                string.IsNullOrWhiteSpace(medico.Crm))
            {
                Console.WriteLine("Preencha todos os campos.");
            }
            else
            {
                medicos.Add(medico);
                Console.WriteLine("Médico cadastrado com sucesso!");
            }

            Pausar();
        }

        static void ListarMedicos()
        {
            Console.Clear();
            Console.WriteLine("=== LISTA DE MÉDICOS ===");

            if (medicos.Count == 0)
                Console.WriteLine("Nenhum médico cadastrado.");

            foreach (Medico m in medicos)
            {
                Console.WriteLine(
                    $"ID: {m.Id} | Nome: {m.Nome} | " +
                    $"Especialidade: {m.Especialidade} | CRM: {m.Crm}"
                );
            }

            Pausar();
        }

        static void AgendarConsulta()
        {
            Console.Clear();
            Console.WriteLine("=== AGENDAMENTO DE CONSULTA ===");

            if (pacientes.Count == 0 || medicos.Count == 0)
            {
                Console.WriteLine(
                    "Cadastre pelo menos um paciente e um médico."
                );
                Pausar();
                return;
            }

            ListarPacientesSemPausa();

            Console.Write("ID do paciente: ");
            if (!int.TryParse(Console.ReadLine(), out int pacienteId))
            {
                Console.WriteLine("ID inválido.");
                Pausar();
                return;
            }

            Paciente? paciente =
                pacientes.FirstOrDefault(p => p.Id == pacienteId);

            if (paciente == null)
            {
                Console.WriteLine("Paciente não encontrado.");
                Pausar();
                return;
            }

            ListarMedicosSemPausa();

            Console.Write("ID do médico: ");
            if (!int.TryParse(Console.ReadLine(), out int medicoId))
            {
                Console.WriteLine("ID inválido.");
                Pausar();
                return;
            }

            Medico? medico =
                medicos.FirstOrDefault(m => m.Id == medicoId);

            if (medico == null)
            {
                Console.WriteLine("Médico não encontrado.");
                Pausar();
                return;
            }

            Console.Write("Data e hora (dd/MM/aaaa HH:mm): ");

            if (!DateTime.TryParseExact(
                Console.ReadLine(),
                "dd/MM/yyyy HH:mm",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out DateTime dataHora))
            {
                Console.WriteLine("Data inválida.");
                Pausar();
                return;
            }

            if (dataHora < DateTime.Now)
            {
                Console.WriteLine("Não é permitido agendar no passado.");
                Pausar();
                return;
            }

            bool horarioOcupado = consultas.Any(c =>
                c.MedicoId == medicoId &&
                c.DataHora == dataHora);

            if (horarioOcupado)
            {
                Console.WriteLine("Horário já ocupado para este médico.");
                Pausar();
                return;
            }

            Consulta consulta = new Consulta
            {
                Id = consultas.Count + 1,
                PacienteId = pacienteId,
                MedicoId = medicoId,
                DataHora = dataHora
            };

            consultas.Add(consulta);

            Console.WriteLine("Consulta agendada com sucesso!");

            Pausar();
        }

        static void ListarConsultas()
        {
            Console.Clear();
            Console.WriteLine("=== CONSULTAS AGENDADAS ===");

            if (consultas.Count == 0)
                Console.WriteLine("Nenhuma consulta cadastrada.");

            foreach (Consulta c in consultas)
            {
                Paciente? paciente =
                    pacientes.FirstOrDefault(p => p.Id == c.PacienteId);

                Medico? medico =
                    medicos.FirstOrDefault(m => m.Id == c.MedicoId);

                Console.WriteLine("----------------------------------");
                Console.WriteLine($"Consulta: {c.Id}");
                Console.WriteLine($"Paciente: {paciente?.Nome}");
                Console.WriteLine($"Médico: {medico?.Nome}");
                Console.WriteLine($"Data: {c.DataHora:dd/MM/yyyy HH:mm}");
            }

            Pausar();
        }

        static void ListarPacientesSemPausa()
        {
            Console.WriteLine("\n=== PACIENTES ===");

            foreach (Paciente p in pacientes)
                Console.WriteLine($"{p.Id} - {p.Nome}");
        }

        static void ListarMedicosSemPausa()
        {
            Console.WriteLine("\n=== MÉDICOS ===");

            foreach (Medico m in medicos)
                Console.WriteLine($"{m.Id} - {m.Nome}");
        }

        static void Pausar()
        {
            Console.WriteLine("\nPressione ENTER para continuar...");
            Console.ReadLine();
        }
    }

    class Paciente
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public int Idade { get; set; }
        public string Telefone { get; set; } = "";
    }

    class Medico
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public string Especialidade { get; set; } = "";
        public string Crm { get; set; } = "";
    }

    class Consulta
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public int MedicoId { get; set; }
        public DateTime DataHora { get; set; }
    }
}

