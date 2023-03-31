

1.Class lib üzerinden Migration alırken ilgii projeyi Nuget Package Manager üzerinden seçelim

2.Add-Migration InitializeMigration -Context BlogAppContext -o "EF/Migrations"

3.Update-database -Context BlogAppContext

Eğer class lib üzerinden Migration alacak isek aşağıdaki paketide yüklememiz gerekecektir.
Microsoft.EntityFrameworkCore.Design

