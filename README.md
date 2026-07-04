# Programeerimine2

## Lühike kirjeldus:

Tegemist on Kooli projektiga kus loome/testime rakendusi millega teeme Add/Save/Delete/Edit päringuid andmebaasile.

## Vajalik:

Visual Stuido Community 2026

- ASP.NET and web development
- .NET desktop development

## How to run:

Ava folder visual studios
sisene KooliProjekt.sln solusionisse.

ava Tools -> NuGet Package Manager -> Package Manager Console
`Update-Database`

View -> Terminal
Ava PowerShell visual studios ning sisesta käsk:
`dotnet run --project KooliProjekt.WebAPI`

### Sealt edasi saab valida 3 rakenduse vahel kus andmeid muuta:
- WindowsForms
- WpfApplication
- BlazorWasm

### Testide hulgas on kokku 192 testi:
- Integratsiooni testid.
- WindowsForms Unit testid
- WpfApplication Unit testid
- Unit testid Handleritele.