# Project Blockchain - A Blockchain Simulation

### Written by Mirsaid Abdullaev, 2024
### Achieved a 100% mark (75/75) in May 2024
<p>My A Level Computer Science NEA project, completed in VB.NET using the Windows Forms application development tools in Visual Studio Community Edition, and compatible with the .NET Framework 4.7.2 SDK.</p>
<p>The completed product features a suite of features available to the user, such as the generation, login and deletion
   of wallets, a bespoke LAN cross-device connection protocol using a linked-list principle, a blockchain explorer,
   ability of users to send and receive the system cryptocurrency, live transaction pool views and status of the current
   state of the network of devices as well as the device role in the network.</p>
<p>The project was accompanied by a fully documented development process across approximately 6 months alongside other A Level study, spanning the
   entire software development lifecycle: Analysis, Design, Implementation, Testing and Evaluation.</p>
   
<span>As part of the development process, I have filmed a full walkthrough video showcasing my application and this can be found on Youtube [here](https://youtu.be/93i-FwiFvy8).</span>

<p>If you have any suggestions or bug fixes or improvements, please submit a pull request or contact me.</p>

## Problem Statement/ Project Outline
<p>This project investigates the feasibility of 
simulating a decentralised, peer-to-peer transactions network (blockchain). The purpose of 
the system is to enable users on a LAN to securely interact with each other, through using 
hashing principles, peer-to-peer data validation and anonymity. The project aims to educate 
users on decentralised finance and explore future potential of creating such a solution.</p>

## Proposed Solution
<p>The solution is going to be programmed in Visual
Basic .NET, in the Visual Studio IDE using the Windows Forms project template for the UI
elements and the event handling of the application. Communicaiton will be carried out using
both TCP and UDP protocols using Visual Basic's System.Net library, and Newtonsoft.Json
external library for the JSON serialisation/deserialisation. The solution will aim to function
across a LAN on Windows-based devices.</p>

## System Design - Data Flow Diagrams and High-Level Overview Diagrams
<p>In the following diagrams, the structure of the
solution application as well as a breakdown of a sample of the key processes within the app are
shown. These diagrams contain the high-level breakdowns of the processes that have been
implemented in the source code.

### Application Form Hierarchy Diagram
<p>For more detail on this, scroll down to the 
application UI overview section to preview the actual form designs and the UI provided by
the application.</p>
<img src="\Project Showcase\Diagrams\Form Navigation Final.png"></img>

### Application Level 0 Data Flow Diagram
<p>Overall system data flow diagram, detailing
all the main processes and their respective data flows that occur in the application. Client
and server-side processes are split up and run parallel to each other as the application is a
distributed P2P network where nodes can all run the same tasks - hence requiring both client
and server-side processes in simultaneous execution.</p>

<img src="\Project Showcase\Diagrams\L0 DFD Final.png"></img>

### Application High-Level Functionality Diagrams
<p>First layman's diagram showing what a user can
do whilst using this application, alongside links with where each task can be accomplished from
within the application form structure, shown to the right of the options and linked by number.</p>

<img src="\Project Showcase\Diagrams\Laymans Diagram 2.png"></img>

<p>Second layman's diagram showing how devices connect
and communicate with each other using my own custom connection/communication protocol within a LAN.
This diagram is essential for understanding the application communications that take place and why
they take place in a specific way, using the predefined JSON requests and responses of my app.</p>

<img src="\Project Showcase\Diagrams\Laymans Diagram 1.png"></img>

### Application Synchronisation Sequence Data Flow Diagram (Level 1)
<span>Level 1 data flow diagram showing the processing of
data from the synchronisation process shown in the overall system data flow diagram (Level 0). The
source code of all the synchronisation subroutines can be accessed in this file: </span>[Synchronisation.vb](./Modules/Synchronisation.vb).
<br><br>
<img src="\Project Showcase\Diagrams\Synchronisation L1 DFD.png"></img>

### Application Local Synchronisation Process Data Flow Diagram (Level 2)
<span>Level 2 data flow diagram showing the processing of
data from the local process shown in the overall synchronisaction data flow diagram (Level 1). Source
code of the actual Local Sync process can be found </span>[here](./Modules/Synchronisation.vb#L131-L188).
<br><br>
<img src="\Project Showcase\Diagrams\Local Sync L2 DFD.png"></img>

### Application Online Synchronisation Process Algorithm Flowchart
<span>Flowchart for the Online Synchronisation algorithm
for the project. Source code link to the actual implementation can be found </span>[here](./Modules/Synchronisation.vb#L69-L129).

<img src="\Project Showcase\Diagrams\Online Sync Flowchart.png"></img>

### Application Connection Sequence Process Diagram
<p>Diagram illustrating the high-level process of two
nodes connecting using the custom JSON messages of the project.</p>

<img src="\Project Showcase\Diagrams\Connection Sequence Updated.png"></img>

## Application UI - Forms and Interface
<p>Following in this section are the individual forms
that make up the project application. Each form is displayed as an image of the form at runtime.</p>

### SyncForm
<p>The initial loading screen form, greeting the user with
a simple connect/synchronise button to begin the synchronisation process and connect to the network of
nodes (if one exists). Progress is shown to the user dynamically through the status label which states
what actions are currently being performed in the back-end, as well as the progress bar updating as each
of the required synchronisation tasks is completed.</p>

<span>Form source code: [SyncForm.vb](./Forms/SyncForm/SyncForm.vb)</span>

<img src="\Project Showcase\App UI\SyncForm.png"></img>

### MainMenu
<p>This is the next form shown to the user after the node has
been connected and synchronised on the blockchain. It shows the user all the main options available to
them - managing wallets, sending cryptocurrency, viewing the blockchain data through the blockchain explorer
and viewing the current live transaction pool of the network.</p>

<span>Form source code: [MainMenu.vb](./Forms/MainMenu/MainMenu.vb)</span>

<img src="\Project Showcase\App UI\MainMenu.png"></img>

### BaseViewingForm
<p>This is the form for the blockchain explorer options - the user
can choose what data to view (either the network status data, or the actual transactions data) as well as
choosing to view the full data (including transactions details) or simply view block headers (without any
details of the transactions, a more concise view).</p>

<span>Form source code: [BaseViewingForm.vb](./Forms/BaseViewingForm/BaseViewingForm.vb)</span>

<img src="\Project Showcase\App UI\BaseViewingForm.png"></img>

### BlockchainExplorerF
<p>This is the full blockchain explorer viewing form - showing all
the contents of every block on the network. Should a new block be mined while  user is on this view, the form
will dynamically add the new block details to the explorer view in real-time through the use of threading.</p>

<span>Form source code: [BlockchainExplorerF.vb](./Forms/BlockchainExplorerF/BlockchainExplorerF.vb)</span>

<img src="\Project Showcase\App UI\BlockchainExplorerF.png"></img>

### BlockchainExplorerH
<p>Same as the previous form, just displays concise data of each
block, only block headers.</p>

<span>Form source code: [BlockchainExplorerH.vb](./Forms/BlockchainExplorerH/BlockchainExplorerH.vb)</span>

<img src="\Project Showcase\App UI\BlockchainExplorerH.png"></img>

### NetStatusView
<p>This form shows which devices the user's node is connected to,
as well as the role of the user's node on the network.</p>

<span>Form source code: [NetStatusView.vb](./Forms/NetworkStatusView/NetStatusView.vb)</span>

<img src="\Project Showcase\App UI\NetStatusView.png"></img>

### WalletBaseView
<p>The main form for managing all wallet-related functionality.
As shown, wallets can be generated, deleted, logged into, or simply viewed (for checking the public address)
from here. Each of these subfunctions is carried out through a dynamic and interactive user input process,
involving custom message and input boxes.</p>

<span>Form source code: [WalletBaseView.vb](./Forms/WalletBaseView/WalletBaseView.vb)</span>

<img src="\Project Showcase\App UI\WalletBaseView.png"></img>

### SendingScreen
<p>This form allows the user to send cryptocurrency to a specified
wallet public address. User inputs are validated, all transactions are checked in the back-end to ensure the user
is not fraudulently sending more cryptocurrency than the wallet they're logged into contains.</p>

<span>Form source code: [SendingScreen.vb](./Forms/SendingScreen/SendingScreen.vb)</span>

<img src="\Project Showcase\App UI\SendingScreen.png"></img>

### TransactPoolView
<p>This form is similar to the BlockchainExplorer viewer forms as it
shows the live transaction pool - transactions that have been sent since the last block has been mined but not
currently in a new block. This is also a real-time updating form and any new transactions get instantly displayed
to the user. Once a block is mined that uses all of these transactions, the pool is flushed, and the transaction
pool view becomes empty again.</p>

<span>Form source code: [TransactPoolView.vb](./Forms/TransactPoolView/TransactPoolView.vb)</span>

<img src="\Project Showcase\App UI\TransactPoolView.png"></img>

## Application JSON Requests and Responses Schemas
<span>Following on from this are the schemas of the custom JSON requests and responses that I have created for use
in the cross-device communications to standardise the data flows from device to device. With each JSON schema, I have
provided some sample data for each of the fields in the JSON schema, to give a feel for what kind of typical data will
be sent and received by devices using this application. These are encapsulated in their separate JSONSchemas module
within the source code file [JSONSchemas.vb](./Modules/JSONSchemas.vb).</span>
## REQUEST SCHEMAS (and some sample data as examples)
**ConnectionRequest**
```
{
"MessageType" : "ConnectionRequest",
"AppIDMessage" : "WAYFARER_V1",
"DeviceIP" : "192.168.1.12",
"RootAddress" : "192.168.1.71"
}
```
**SyncRequest**
```
{
"MessageType" : "SyncRequest",
"AppIDMessage" : "WAYFARER_V1",
"DeviceIP" : "192.168.1.12",
"RootAddress" : "192.168.1.71",
"StartBlock" : "253"
}
```
**NewTransactionRequest**
```
{
"MessageType" : "NewTransactionRequest",
"AppIDMessage" : "WAYFARER_V1",
"DeviceIP" : "192.168.1.12",
"RootAddress" : "192.168.1.71",
"Timestamp" : "1717085175123",
"Sender" : "AE283F...C10",
"Recipient" : "75483...BA1",
"Quantity" : "95",
"Fee" : "5"
}
```
**ValidateNewMinedBlockRequest**
```
{
"MessageType" : "ValidateNewMinedBlockRequest",
"AppIDMessage" : "WAYFARER_V1",
"DeviceIP" : "192.168.1.12",
"RootAddress" : "192.168.1.71",
"Timestamp" : "1717085175123",
"PrevHash" : "12FF3...BDD",
"Hash" : "95201...29C",
"Index" : "95",
"Nonce" : "120021",
"Transactions" : "[transact1],[transact2]...[transactN]",
"Miner" : "AE283F...C10"
}
```
**TransmitNewBlockRequest**
```
{
"MessageType" : "TransmitNewBlockRequest",
"AppIDMessage" : "WAYFARER_V1",
"DeviceIP" : "192.168.1.12",
"RootAddress" : "192.168.1.71",
"Timestamp" : "1717085175123",
"PrevHash" : "12FF3...BDD",
"Hash" : "95201...29C",
"Index" : "95",
"Nonce" : "120021",
"Transactions" : "[transact1],[transact2]...[transactN]",
"Miner" : "AE283F...C10"
}
```
**TransmitTransactionRequest**
```
{
"MessageType" : "TransmitTransactionRequest",
"AppIDMessage" : "WAYFARER_V1",
"DeviceIP" : "192.168.1.12",
"RootAddress" : "192.168.1.71",
"Timestamp" : "1717085175123",
"Sender" : "AE283F...C10",
"Recipient" : "75483...BA1",
"Quantity" : "95",
"Fee" : "5"
}
```
**DisconnectRequest**
```
{
"MessageType" : "DisconnectRequest",
"AppIDMessage" : "WAYFARER_V1",
"DeviceIP" : "192.168.1.12",
"RootAddress" : "192.168.1.71"
}
```
## RESPONSE SCHEMAS (and some sample data as examples)
**ConnectionResponse**
```
{
"MessageType" : "ConnectionResponse",
"AppIDMessage" : "WAYFARER_V1",
"DeviceIP" : "192.168.1.12",
"Status" : "Accept"
}
```
**SyncResponse**
```
{
"MessageType" : "SyncResponse",
"AppIDMessage" : "WAYFARER_V1",
"DeviceIP" : "192.168.1.12",
"Status" : "Accept",
"ExpectedBlocks" : "152"
}
```
**NewTransactionResponse**
```
{
"MessageType" : "NewTransactionResponse",
"AppIDMessage" : "WAYFARER_V1",
"DeviceIP" : "192.168.1.12",
"Status" : "Accept"
}
```
**ValidateNewMinedBlockResponse**
```
{
"MessageType" : "ValidateNewMinedBlockResponse",
"AppIDMessage" : "WAYFARER_V1",
"DeviceIP" : "192.168.1.12",
"Status" : "Accept"
}
```
**TransactionPoolResponse**
```
{
"MessageType" : "TransactionPoolResponse",
"AppIDMessage" : "WAYFARER_V1",
"DeviceIP" : "192.168.1.12",
"Status" : "Accept",
"Transactions" : "[transact1],[transact2]...[transactN]"
}
```
**BlockResponse**
```
{
"MessageType" : "BlockResponse",
"AppIDMessage" : "WAYFARER_V1",
"DeviceIP" : "192.168.1.12",
"Status" : "Accept",
"Timestamp" : "1717085175123",
"PrevHash" : "12FF3...BDD",
"Hash" : "95201...29C",
"Index" : "95",
"Nonce" : "120021",
"Transactions" : "[transact1],[transact2]...[transactN]",
"Miner" : "AE283F...C10"
}
```
## Complex Code Techniques Used - According to AQA's Coursework Specification*
<span>*AQA Non-Exam Assessment in A Level Computer Science has a fully detailed mark scheme and specification where all requirements and details on what is expected from students is laid out. This document can be found here: [https://www.aqa.org.uk/subjects/computer-science-and-it/as-and-a-level/computer-science-7516-7517/subject-content-a-level/non-exam-assessment-the-computing-practical-project](https://www.aqa.org.uk/subjects/computer-science-and-it/as-and-a-level/computer-science-7516-7517/subject-content-a-level/non-exam-assessment-the-computing-practical-project)</span>

<p>The following table shows a sample of the complex techniques that I have demonstrated within the source code of this project. This is by no means an exhaustive list, neither is it a fully descriptive list, but rather these are the techniques that map to AQA's specifications, and therefore only represent a subsection of my complete project development.</p>

<img src="/Project Showcase/Diagrams/Complex Techniques.png"></img>

## Conclusion: Final reflection and future outlook*
<span>*This excerpt is taken from my project evaluation of my final coursework submission, and it is fitting to include here as a brief and informative way to summarise the journey of developing this project. Note that some references in this exceprt might be to objectives/features I have not discussed here, as this was part of my overall full coursework submission, which spanned over 200 pages, and this would be too much to include on here. The exceprt is appropriate here regardless.</span>

*Overall, I believe that my project and solution has achieved its set-out objectives to an acceptable level, and the evidence-based testing of the system has showcased its success. At its core, I wanted to build a simulation for a blockchain, and show the feasibility of creating a larger-scale payments/transactions network focused on* *decentralised principles. Taking inspiration from Bitcoin, Ethereum and other current blockchain systems, I used existing solutions to formulate a bespoke blockchain solution in VB.NET, a language not used in most practical applications, and*
*also plan and implement the logical network topology for communications.*

*Arguably, the network communications and defensive design implications due to the network communications were objectives which required the most time to conceptualise and implement, as each device would have the exact same software running. This had wide-ranging implications, such as the logical sorting of client and server processes in* *parallel. It is due to this objective that I have had to redesign my final solution multiple times, after carrying out practical tests on the initial idea for the network, I made the conclusion that the data communications was inadequately structured. Hence the introduction of JSON to format communications. Furthermore, my initial* *solution required that all devices broadcast data packets to recognise each other on the network. However, this had the unintended consequences of acting as a flood attack on the LAN that it was run on, as well as the mess of sorting which devices were online at a given time and the connections between nodes. This spurred me to create* *the linked list of nodes idea, and this drastically cleared up both network congestion, as well as data transfer efficiency, meaning that redundant data is not sent around the LAN, taking up bandwidth.* 

*As for the functionality of the project solution itself, I am overall happy with the suite of functions available for the end user, and the success of the testing phase has showcased the ease-of-use and the performance of the blockchain system. I believe that my small-scale project has concisely showcased the possibilities of blockchain* *and can be used as a tool to learn more about the subject. This claim has also been supported by [my project end user] and his feedback regarding my final solution.* 

*Despite this, I would have liked to implement ideas set out in my secondary objectives as an extension to the project, and the implementation of those objectives would have increased the scope of my project as many nodes in different LANs would have been able to use the blockchain system together, more accurately reflecting real-life* *blockchain solutions. However, due to the scope of the secondary objectives mostly being outside of any A-Level standards, such as establishing a peer-to-peer network over the WAN (secondary objective #5), I took the decision to focus myself on the A-Level focused primary objectives and make sure that the logical and theoretical concepts regarding the core blockchain operations were able to be mapped to real-world successful operation. This blockchain solution now provides me with a starting point in expanding the project scope and possibly building a blockchain on a larger scale.*

### Written by Mirsaid Abdullaev, 2024
## Contact Details
### Email: abdullaevm017@gmail.com
### LinkedIn: www.linkedin.com/in/mirsaid-abdullaev-6a4ab5242/
