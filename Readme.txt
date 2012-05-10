NStub (TNG), a fork by Jedzia.
=================================

NStub is invented by Jeremy Jarrell. His original work is available on Google Developers http://code.google.com/p/nstub/ with
"svn checkout http://nstub.googlecode.com/svn/trunk/ nstub-read-only" as source repository. This fork is my private work
to ease my life when auto-generating unit-tests for RhinoMocks. 
From Jeremy's description of NStub:

NStub? What's NStub? 
NStub is a unit test generator for .NET assemblies. You simply point NStub at the assembly containing the types you wish to test, 
pick the types and methods that your interested in from interface, choose where you want the resulting source files to be created at,
and click GO!. That's all there is to it!

What do I get? Your resulting group of source files will contain one stubbed test method for each method that you selected. 
The code is clean, clearly documented and is already decorated to work with your chosen test framework. All you have to do is 
fill in the logic. Each group of source files even comes with an associated project allowing you to quickly open the files in 
Visual Studio and start editing. Not using Visual Studio? No problem! Just open the files individually in your chosen development 
environment and go to work.

That sounds great...except for... NStub is fully extensible, allowing you to create plug-ins not only to support multiple testing 
frameworks but also to generate the testing code in any .NET compliant language. The sky is the limit to what you can accomplish.

There a few driving principles behind NStub, we consider these our 'bill of rights' so to speak and hold them 
in highest regarding when making all design and development decisions. No matter what functionality is added to NStub, you can 
always rest assured that none of these inalienable rights will ever be violated.

   - All projects will compile immediately after generation
   - NStub will not 'lock you into' any particular testing framework or language
   - There will be no aspect of NStub that you can't extend, alter, or modify to suit your own purposes 

This sounds like something I could use! Great, then give it a shot! Just download our current stable build and let us know what you think.
Have a suggestion or want to help out? Even better! Just drop us a line and we'll be happy to listen to what you have to say! 


---------------

Just to reinvent the wheel i'm trying to make that wonderful test generator a little bit more modular and extend it to spit out
most repeatable tasks in your allday unit test keyboard hacking.
Mr. Jarrell published his work under the New BSD License, so do i. The exact wording of the license is available from
http://www.opensource.org/licenses/bsd-license.php. Feel happy by using NStub and/or help me in development. Proposals 
and suggestions for improvement are always welcome.
