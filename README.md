LanHouseManager
===============
*Descrição do Projeto*

Trabalho de conclusão de curso da faculdade de Ciências da Computação 2009 UNISANTA.

* .NET
* C#
* WPF
* ASP.NET MVC
* Entity Framework
* SQL Server

!!! Documento TCC: [file:Lan_Manager.pdf]
!!! Visualizar demonstações: [Videos]

!! *Descrição do Projeto*

O sistema trabalha somente com o modo pré-pago, ou seja, o usuário paga antes de usar o computador, se a hora for R$ 2,00, quando o cliente pagar R$2,00, o sistema vai entender que o usuário tem 60 minutos de crédito. Será usado dinheiro e não minutos na hora de adicionar o crédito por conta da emissão da nota fiscal eletrônica. 

O sistema está totalmente enquadrado dentro dos requisitos da nova lei para Lan House do estado de São Paulo.

Na parte de segurança do software o sistema cria uma nova área de trabalho para o cliente, sem acesso ao sistema operacional, limitando o uso dos softwares disponibilizados pelo sistema. Quando a sessão do cliente é finalizada, todos os aplicativos aberto por ele também serão fechado, garantindo que o próximo cliente não acesse os aplicativos.

O sistema trata casos como reinicio do computador ou falta de energia, mantendo a mesma sessão ou fechando automaticamente. 

O sistema é composto por três módulos, Servidor, Estação e Web.

No módulo Estação o nosso sistema “trava” a máquina totalmente, somente sendo liberado com login e senha do cliente cadastrado.

[image:Estacao.PNG]

No módulo Web os pais podem ver os relatórios de uso de seus dependentes e alterar as permissões de acesso.

[image:Web1.PNG]
[image:Web2.PNG]
[image:Web3.PNG]

Já no módulo servidor, o sistema monitora todas as máquinas usadas, sendo responsável por toda a parte administrativa, que vai desde o cadastro dos clientes, adicionar créditos ao cliente, bloquear cliente, gerar relatório com as estatística de uso dos micros, etc.

[image:Cadastro.PNG]
[image:Diario.PNG]
[image:Sessoes.PNG]
