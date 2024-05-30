# Project Blockchain

## Problem Statement/ Project Outline
<p style="font-family:'Arial';font-size:110%">This project investigates the feasibility of 
simulating a decentralised, peer-to-peer transactions network (blockchain). The purpose of 
the system is to enable users on a LAN to securely interact with each other, through using 
hashing principles, peer-to-peer data validation and anonymity. The project aims to educate 
users on decentralised finance and explore future potential of creating such a solution.</p>

## Proposed Solution
<p style="font-family:'Arial';font-size:110%">The solution is going to be programmed in Visual
Basic .NET, in the Visual Studio IDE using the Windows Forms project template for the UI
elements and the event handling of the application. Communicaiton will be carried out using
both TCP and UDP protocols using Visual Basic's System.Net library, and Newtonsoft.Json
external library for the JSON serialisation/deserialisation. The solution will aim to function
across a LAN on Windows-based devices.</p>

## System Design - Data Flow Diagrams and High-Level Overview Diagrams
<p style="font-family:'Arial';font-size:110%">In the following diagrams, the structure of the
solution application as well as a breakdown of a sample of the key processes within the app are
shown. These diagrams contain the high-level breakdowns of the processes that have been
implemented in the source code.

### Application Form Hierarchy Diagram
<p style="font-family:'Arial';font-size:110%">For more detail on this, scroll down to the 
application UI overview section to preview the actual form designs and the UI provided by
the application.</p>
<img src="\Project Showcase\Diagrams\Form Navigation Final.png"></img>

### Application Level 0 Data Flow Diagram
<p style="font-family:'Arial';font-size:110%">Overall system data flow diagram, detailing
all the main processes and their respective data flows that occur in the application. Client
and server-side processes are split up and run parallel to each other as the application is a
distributed P2P network where nodes can all run the same tasks - hence requiring both client
and server-side processes in simultaneous execution.</p>

<img src="\Project Showcase\Diagrams\L0 DFD Final.png"></img>

### Application High-Level Functionality Diagrams
<p style="font-family:'Arial';font-size:110%">First layman's diagram showing what a user can
do whilst using this application, alongside links with where each task can be accomplished from
within the application form structure, shown to the right of the options and linked by number.</p>

<img src="\Project Showcase\Diagrams\Laymans Diagram 2.png"></img>

<p style="font-family:'Arial';font-size:110%">Second layman's diagram showing how devices connect
and communicate with each other using my own custom connection/communication protocol within a LAN.
This diagram is essential for understanding the application communications that take place and why
they take place in a specific way, using the predefined JSON requests and responses of my app.</p>

<img src="\Project Showcase\Diagrams\Laymans Diagram 1.png"></img>

### Application Synchronisation Sequence Data Flow Diagram (Level 1)
<p style="font-family:'Arial';font-size:110%">Level 1 data flow diagram showing the processing of
data from the synchronisation process shown in the overall system data flow diagram (Level 0). The
source code of all the synchronisation subroutines can be accessed in this file, [Synchronisation.vb](./Modules/Synchronisation.vb)</p>

<img src="\Project Showcase\Diagrams\Synchronisation L1 DFD.png"></img>

### Application Local Synchronisation Process Data Flow Diagram (Level 2)
<p style="font-family:'Arial';font-size:110%">Level 2 data flow diagram showing the processing of
data from the local process shown in the overall synchronisaction data flow diagram (Level 1). Source
code of the actual Local Sync process can be found [here](./Modules/Synchronisation.vb#L131-L214)</p>

<img src="\Project Showcase\Diagrams\Local Sync L2 DFD.png"></img>

### Application Online Synchronisation Process Algorithm Flowchart
<p style="font-family:'Arial';font-size:110%">Flowchart for the Online Synchronisation algorithm
for the project. Source code link to the actual implementation can be found [here](./Modules/Synchronisation.vb#L69-L129)</p>

<img src="\Project Showcase\Diagrams\Online Sync Flowchart.png"></img>

### Application Connection Sequence Process Diagram
<p style="font-family:'Arial';font-size:110%">Diagram illustrating the high-level process of two
nodes connecting using the custom JSON messages of the project.</p>

<img src="\Project Showcase\Diagrams\Connection Sequence Updated.png"></img>

## Application UI - Forms and Interface
<p style="font-family:'Arial';font-size:110%">Following in this section are the individual forms
that make up the project application. Each form is displayed as an image of the form at runtime.</p>

### SyncForm
<p style="font-family:'Arial';font-size:110%">The initial loading screen form, greeting the user with
a simple connect/synchronise button to begin the synchronisation process and connect to the network of
nodes (if one exists). Progress is shown to the user dynamically through the status label which states
what actions are currently being performed in the back-end, as well as the progress bar updating as each
of the required synchronisation tasks is completed.</p>

<p style="font-family:'Arial';font-size:110%">Form source code: [SyncForm.vb](./Forms/SyncForm/SyncForm.vb)</p>

<img src="\Project Showcase\App UI\SyncForm.png"></img>

### MainMenu
<p style="font-family:'Arial';font-size:110%">This is the next form shown to the user after the node has
been connected and synchronised on the blockchain. It shows the user all the main options available to
them - managing wallets, sending cryptocurrency, viewing the blockchain data through the blockchain explorer
and viewing the current live transaction pool of the network.</p>

<p style="font-family:'Arial';font-size:110%">Form source code: [MainMenu.vb](./Forms/MainMenu/MainMenu.vb)</p>

<img src="\Project Showcase\App UI\MainMenu.png"></img>

### BaseViewingForm
<p style="font-family:'Arial';font-size:110%">This is the form for the blockchain explorer options - the user
can choose what data to view (either the network status data, or the actual transactions data) as well as
choosing to view the full data (including transactions details) or simply view block headers (without any
details of the transactions, a more concise view).</p>

<p style="font-family:'Arial';font-size:110%">Form source code: [BaseViewingForm.vb](./Forms/BaseViewingForm/BaseViewingForm.vb)</p>

<img src="\Project Showcase\App UI\BaseViewingForm.png"></img>

### BlockchainExplorerF
<p style="font-family:'Arial';font-size:110%">This is the full blockchain explorer viewing form - showing all
the contents of every block on the network. Should a new block be mined while  user is on this view, the form
will dynamically add the new block details to the explorer view in real-time through the use of threading.</p>

<p style="font-family:'Arial';font-size:110%">Form source code: [BlockchainExplorerF.vb](./Forms/BlockchainExplorerF/BlockchainExplorerF.vb)</p>

<img src="\Project Showcase\App UI\BlockchainExplorerF.png"></img>

### BlockchainExplorerH
<p style="font-family:'Arial';font-size:110%">Same as the previous form, just displays concise data of each
block, only block headers.</p>

<p style="font-family:'Arial';font-size:110%">Form source code: [BlockchainExplorerH.vb](./Forms/BlockchainExplorerH/BlockchainExplorerH.vb)</p>

<img src="\Project Showcase\App UI\BlockchainExplorerH.png"></img>

### NetStatusView
<p style="font-family:'Arial';font-size:110%">This form shows which devices the user's node is connected to,
as well as the role of the user's node on the network.</p>

<p style="font-family:'Arial';font-size:110%">Form source code: [NetStatusView.vb](./Forms/NetworkStatusView/NetStatusView.vb)</p>

<img src="\Project Showcase\App UI\NetStatusView.png"></img>

### WalletBaseView
<p style="font-family:'Arial';font-size:110%">The main form for managing all wallet-related functionality.
As shown, wallets can be generated, deleted, logged into, or simply viewed (for checking the public address)
from here. Each of these subfunctions is carried out through a dynamic and interactive user input process,
involving custom message and input boxes.</p>

<p style="font-family:'Arial';font-size:110%">Form source code: [WalletBaseView.vb](./Forms/WalletBaseView/WalletBaseView.vb)</p>

<img src="\Project Showcase\App UI\WalletBaseView.png"></img>

### SendingScreen
<p style="font-family:'Arial';font-size:110%">This form allows the user to send cryptocurrency to a specified
wallet public address. User inputs are validated, all transactions are checked in the back-end to ensure the user
is not fraudulently sending more cryptocurrency than the wallet they're logged into contains.</p>

<p style="font-family:'Arial';font-size:110%">Form source code: [SendingScreen.vb](./Forms/SendingScreen/SendingScreen.vb)</p>

<img src="\Project Showcase\App UI\SendingScreen.png"></img>

### TransactPoolView
<p style="font-family:'Arial';font-size:110%">This form is similar to the BlockchainExplorer viewer forms as it
shows the live transaction pool - transactions that have been sent since the last block has been mined but not
currently in a new block. This is also a real-time updating form and any new transactions get instantly displayed
to the user. Once a block is mined that uses all of these transactions, the pool is flushed, and the transaction
pool view becomes empty again.</p>

<p style="font-family:'Arial';font-size:110%">Form source code: [TransactPoolView.vb](./Forms/TransactPoolView/TransactPoolView.vb)</p>

<img src="\Project Showcase\App UI\TransactPoolView.png"></img>

## Application JSON Requests and Responses Schemas
### REQUESTS
<p style="background-color: black;font-family:'Consolas'">
<span style="color: #FF007F;font-size:150%">ConnectionRequest</span>
<br>
<span style="color: #22863A">{<br>&nbsp "MessageType" : "ConnectionRequest",
                               <br>&nbsp "AppIDMessage" : "WAYFARER_V1",
                               <br>&nbsp "DeviceIP" : "192.168.1.12",
                               <br>&nbsp "RootAddress" : "192.168.1.71"
                               <br>}</span>
<br>
<span style="color: #FF007F;font-size:150%">SyncRequest</span>
<br>
<span style="color: #22863A">{<br>&nbsp "MessageType" : "SyncRequest",
                               <br>&nbsp "AppIDMessage" : "WAYFARER_V1",
                               <br>&nbsp "DeviceIP" : "192.168.1.12",
                               <br>&nbsp "RootAddress" : "192.168.1.71",
                               <br>&nbsp "StartBlock" : "253"
                               <br>}</span>
<br>
<span style="color: #FF007F;font-size:150%">NewTransactionRequest</span>
<br>
<span style="color: #22863A">{<br>&nbsp "MessageType" : "NewTransactionRequest",
                               <br>&nbsp "AppIDMessage" : "WAYFARER_V1",
                               <br>&nbsp "DeviceIP" : "192.168.1.12",
                               <br>&nbsp "RootAddress" : "192.168.1.71",
                               <br>&nbsp "Timestamp" : "1717085175123",
                               <br>&nbsp "Sender" : "AE283F...C10",
                               <br>&nbsp "Recipient" : "75483...BA1",
                               <br>&nbsp "Quantity" : "95",
                               <br>&nbsp "Fee" : "5"
                               <br>}</span>
<br>
<span style="color: #FF007F;font-size:150%">ValidateNewMinedBlockRequest</span>
<br>
<span style="color: #22863A">{<br>&nbsp "MessageType" : "ValidateNewMinedBlockRequest",
                               <br>&nbsp "AppIDMessage" : "WAYFARER_V1",
                               <br>&nbsp "DeviceIP" : "192.168.1.12",
                               <br>&nbsp "RootAddress" : "192.168.1.71",
                               <br>&nbsp "Timestamp" : "1717085175123",
                               <br>&nbsp "PrevHash" : "12FF3...BDD",
                               <br>&nbsp "Hash" : "95201...29C",
                               <br>&nbsp "Index" : "95",
                               <br>&nbsp "Nonce" : "120021",
                               <br>&nbsp "Transactions" : "[transact1],[transact2]...[transactN]",
                               <br>&nbsp "Miner" : "AE283F...C10"
                               <br>}</span>
<br>
<span style="color: #FF007F;font-size:150%">TransmitNewBlockRequest</span>
<br>
<span style="color: #22863A">{<br>&nbsp "MessageType" : "TransmitNewBlockRequest",
                               <br>&nbsp "AppIDMessage" : "WAYFARER_V1",
                               <br>&nbsp "DeviceIP" : "192.168.1.12",
                               <br>&nbsp "RootAddress" : "192.168.1.71",
                               <br>&nbsp "Timestamp" : "1717085175123",
                               <br>&nbsp "PrevHash" : "12FF3...BDD",
                               <br>&nbsp "Hash" : "95201...29C",
                               <br>&nbsp "Index" : "95",
                               <br>&nbsp "Nonce" : "120021",
                               <br>&nbsp "Transactions" : "[transact1],[transact2]...[transactN]",
                               <br>&nbsp "Miner" : "AE283F...C10"
                               <br>}</span>
<br>
<span style="color: #FF007F;font-size:150%">TransmitTransactionRequest</span>
<br>
<span style="color: #22863A">{<br>&nbsp "MessageType" : "TransmitTransactionRequest",
                               <br>&nbsp "AppIDMessage" : "WAYFARER_V1",
                               <br>&nbsp "DeviceIP" : "192.168.1.12",
                               <br>&nbsp "RootAddress" : "192.168.1.71",
                               <br>&nbsp "Timestamp" : "1717085175123",
                               <br>&nbsp "Sender" : "AE283F...C10",
                               <br>&nbsp "Recipient" : "75483...BA1",
                               <br>&nbsp "Quantity" : "95",
                               <br>&nbsp "Fee" : "5"
                               <br>}</span>
<br>
<span style="color: #FF007F;font-size:150%">DisconnectRequest</span>
<br>
<span style="color: #22863A">{<br>&nbsp "MessageType" : "DisconnectRequest",
                               <br>&nbsp "AppIDMessage" : "WAYFARER_V1",
                               <br>&nbsp "DeviceIP" : "192.168.1.12",
                               <br>&nbsp "RootAddress" : "192.168.1.71"
                               <br>}</span>
<br>
</p>

### RESPONSES
<p style="background-color: black;font-family:'Consolas'">
<span style="color: #FF007F;font-size:150%">ConnectionResponse</span>
<br>
<span style="color: #22863A">{<br>&nbsp "MessageType" : "ConnectionResponse",
                               <br>&nbsp "AppIDMessage" : "WAYFARER_V1",
                               <br>&nbsp "DeviceIP" : "192.168.1.12",
                               <br>&nbsp "Status" : "Accept"
                               <br>}</span>
<br>
<span style="color: #FF007F;font-size:150%">SyncResponse</span>
<br>
<span style="color: #22863A">{<br>&nbsp "MessageType" : "SyncResponse",
                               <br>&nbsp "AppIDMessage" : "WAYFARER_V1",
                               <br>&nbsp "DeviceIP" : "192.168.1.12",
                               <br>&nbsp "Status" : "Accept",
                               <br>&nbsp "ExpectedBlocks" : "152"
                               <br>}</span>
<br>
<span style="color: #FF007F;font-size:150%">NewTransactionResponse</span>
<br>
<span style="color: #22863A">{<br>&nbsp "MessageType" : "NewTransactionResponse",
                               <br>&nbsp "AppIDMessage" : "WAYFARER_V1",
                               <br>&nbsp "DeviceIP" : "192.168.1.12",
                               <br>&nbsp "Status" : "Accept"
                               <br>}</span>
<br>
<span style="color: #FF007F;font-size:150%">ValidateNewMinedBlockResponse</span>
<br>
<span style="color: #22863A">{<br>&nbsp "MessageType" : "ValidateNewMinedBlockResponse",
                               <br>&nbsp "AppIDMessage" : "WAYFARER_V1",
                               <br>&nbsp "DeviceIP" : "192.168.1.12",
                               <br>&nbsp "Status" : "Accept"
                               <br>}</span>
<br>
<span style="color: #FF007F;font-size:150%">TransactionPoolResponse</span>
<br>
<span style="color: #22863A">{<br>&nbsp "MessageType" : "TransactionPoolResponse",
                               <br>&nbsp "AppIDMessage" : "WAYFARER_V1",
                               <br>&nbsp "DeviceIP" : "192.168.1.12",
                               <br>&nbsp "Status" : "Accept",
                               <br>&nbsp "Transactions" : "[transact1],[transact2]...[transactN]"
                               <br>}</span>
<br>
<span style="color: #FF007F;font-size:150%">BlockResponse</span>
<br>
<span style="color: #22863A">{<br>&nbsp "MessageType" : "BlockResponse",
                               <br>&nbsp "AppIDMessage" : "WAYFARER_V1",
                               <br>&nbsp "DeviceIP" : "192.168.1.12",
                               <br>&nbsp "Status" : "Accept",
                               <br>&nbsp "Timestamp" : "1717085175123",
                               <br>&nbsp "PrevHash" : "12FF3...BDD",
                               <br>&nbsp "Hash" : "95201...29C",
                               <br>&nbsp "Index" : "95",
                               <br>&nbsp "Nonce" : "120021",
                               <br>&nbsp "Transactions" : "[transact1],[transact2]...[transactN]",
                               <br>&nbsp "Miner" : "AE283F...C10"
                               <br>}</span>
<br>
</p>

## Complex Code Techniques Used - According to AQA's Coursework Specification*
<p style="font-size:75%">*AQA Non-Exam Assessment in A Level Computer Science has a fully detailed mark scheme and specification where all requirements and details on what is expected from students is laid out. This document can be found here: [https://www.aqa.org.uk/subjects/computer-science-and-it/as-and-a-level/computer-science-7516-7517/subject-content-a-level/non-exam-assessment-the-computing-practical-project](https://www.aqa.org.uk/subjects/computer-science-and-it/as-and-a-level/computer-science-7516-7517/subject-content-a-level/non-exam-assessment-the-computing-practical-project)

<p style="font-family:'Arial';font-size:110%">The following table shows a sample of the complex techniques that I have demonstrated within the source code of this project. This is by no means an exhaustive list, neither is it a fully descriptive list, but rather these are the techniques that map to AQA's specifications, and therefore only represent a subsection of my complete project development.</p>

<img src="/Project Showcase/Diagrams/Complex Techniques.png"></img>

### Written by Mirsaid Abdullaev, 2024
<p>My A Level Computer Science NEA project, completed in VB.NET using the Windows Forms application development tools in Visual Studio Community Edition, and compatible with the .NET Framework 4.7.2 SDK.</p>
<p>The completed product features a suite of features available to the user, such as the generation, login and deletion
   of wallets, a bespoke LAN cross-device connection protocol using a linked-list principle, a blockchain explorer,
   ability of users to send and receive the system cryptocurrency, live transaction pool views and status of the current
   state of the network of devices as well as the device role in the network.</P>
<P>The project was accompanied by a fully documented development process across approximately 6 months alongside other A Level study, spanning the
   entire software development lifecycle: Analysis, Design, Implementation, Testing and Evaluation. The PDF file of my
   final submission is saved in this repository, titled "Blockchain Wayfarer: Mirsaid Abdullaev". Click <a href="/Blockchain%20Wayfarer%20MA.pdf">here</a>
   to download the PDF file in raw format. Note - it is password protected, but I can share it if contacted.</p>
<p>As part of the development process, I have filmed a full walkthrough video showcasing my application and this can be found on Youtube
   <a href="https://youtu.be/93i-FwiFvy8">here</a>. Also shown here below:</p>

<p align="center">
    <iframe width="560" height="315" src="https://www.youtube.com/embed/93i-FwiFvy8?si=cRjFLhE11fMcPXrp" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share" referrerpolicy="strict-origin-when-cross-origin" allowfullscreen></iframe>
</p>

<p>If you have any suggestions or bug fixes or improvements, please submit a pull request or contact me.</p>
