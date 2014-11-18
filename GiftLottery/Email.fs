namespace GitHub.Lventre.GitLottery

open System
open System.ComponentModel
open System.Configuration
open System.Net.Mail
open System.Text

/// The type responsible for generating and sending the email messages.
type Email () =
    
    /// Email account sending the emails.
    let fromEmail = ConfigurationManager.AppSettings.["fromEmail"]        

    /// Display name of the account sending the emails.
    let fromDisplayName = ConfigurationManager.AppSettings.["fromDisplayName"]

    /// The SMTP client
    let smtpClient =
        let client = new SmtpClient()

        let processCompleted (arg:AsyncCompletedEventArgs) =
            if arg.Cancelled then
                cprintfn ConsoleColor.Yellow "Email cancelled!"
            elif arg.Error <> null then
                arg.Error.Message |> cprintfn ConsoleColor.Red "An error occured while sending email: %s"
            else
                ()

        client.SendCompleted.Add processCompleted
        client

    /// <summary>
    /// Generates the mail message
    /// </summary>
    /// <param name="toParticipant">Participant giving the present, hence receiving the email.</param>
    /// <param name="forParticipant">Participant receiving the present.</param>
    let message toParticipant forParticipant =
        let encoding = Encoding.UTF8
        let fromAddr = MailAddress(fromEmail, fromDisplayName, encoding)
        let toAddr = MailAddress(toParticipant.Email, toParticipant.Name, encoding)
        
        let msg = new MailMessage(fromAddr, toAddr)
        msg.Subject <- "Résultat du tirage au sort de Noël"
        msg.DeliveryNotificationOptions <- DeliveryNotificationOptions.OnFailure
        msg.BodyEncoding <- encoding
        msg.IsBodyHtml <- false
        msg.Body <- sprintf """Bonjour %s,

Voici le résultat du tirage au sort automatique : tu devras faire un cadeau à %s.

Bonne journée.""" toParticipant.Name forParticipant.Name
        msg

    /// <summary>
    /// Sends a message to a participant indicating to whom make a present.
    /// </summary>
    /// <param name="giver">The one making the present.</param>
    /// <param name="receiver">The one receiving the present.</param>
    member this.Send (giver, receiver): unit =
        let msg = message giver receiver
        cprintfn ConsoleColor.DarkGreen "\tSending message to %s" giver.Name
        smtpClient.Send(msg)

    interface IDisposable with

        /// Dispose of the SMTP client.
        member this.Dispose(): unit = 
            smtpClient.Dispose()
